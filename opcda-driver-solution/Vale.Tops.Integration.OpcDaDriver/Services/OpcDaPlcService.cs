
using Opc.Da;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager; // WriteReadContext

namespace Vale.Tops.Integration.OpcDaDriver.Services
{
    public class OpcDaPlcService : ServiceBase
    {
        private readonly string _serviceName;
        private readonly int _flushMeasuresMs;
        private readonly int _pollWritesMs;
        private readonly int _reconnectInitialMs;
        private readonly int _reconnectMaxMs;
        private readonly string _itemPrefix;
        private readonly string _itemSeparator;
        private readonly string _itemSuffix;

        private CancellationTokenSource _cts;
        private Task _workerFlush;
        private Task _workerWrites;

        private Opc.OpcDaClient _opc;
        private Item[] _opcItems;
        private readonly ConcurrentQueue<(long TagId, string Value, DateTime Ts)> _qMeasures = new();

        private Dictionary<long, string> _tagIdToItemId;
        private Dictionary<long, long> _tagIdToMeasureRowId;
        private HashSet<long> _myTagIds;
        private string _opcServerProgId;
        private string _opcServerHost;
        private int _opcUpdateRate;
        private string _topicName;

        public OpcDaPlcService(string serviceName)
        {
            _serviceName = string.IsNullOrWhiteSpace(serviceName) ? (ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver") : serviceName;
            ServiceName = _serviceName;

            _flushMeasuresMs = int.Parse(ConfigurationManager.AppSettings["Timers.FlushMeasuresMs"] ?? "300");
            _pollWritesMs = int.Parse(ConfigurationManager.AppSettings["Timers.PollWritesMs"] ?? "500");
            _reconnectInitialMs = int.Parse(ConfigurationManager.AppSettings["Timers.ReconnectInitialMs"] ?? "2000");
            _reconnectMaxMs = int.Parse(ConfigurationManager.AppSettings["Timers.ReconnectMaxMs"] ?? "60000");

            _itemPrefix = ConfigurationManager.AppSettings["ItemId.Prefix"] ?? string.Empty;
            _itemSeparator = ConfigurationManager.AppSettings["ItemId.Separator"] ?? ".";
            _itemSuffix = ConfigurationManager.AppSettings["ItemId.Suffix"] ?? string.Empty;
        }

#if DEBUG
        public void DebugRun()
        {
            OnStart(null);
            Console.WriteLine($"[{_serviceName}] DEBUG mode. Press ENTER to stop...");
            Console.ReadLine();
            OnStop();
        }
#endif

        protected override void OnStart(string[] args)
        {
            _cts = new CancellationTokenSource();
            LogInfo("Service starting...");

            LoadConfigurationAndTags();

            _opc = new Opc.OpcDaClient(_opcServerHost, _opcServerProgId, _opcUpdateRate, _serviceName);
            _opc.OnDataChanged += HandleDataChanged;
            _opc.OnDisconnected += HandleDisconnected;

            Task.Run(() => ConnectWithRetryAsync(_cts.Token));
            _workerFlush = Task.Run(() => FlushMeasuresLoopAsync(_cts.Token));
            _workerWrites = Task.Run(() => PollWritesLoopAsync(_cts.Token));

            LogInfo($"Service started. Subscribing {_myTagIds.Count} tags on topic '{_topicName}'.");
        }

        protected override void OnStop()
        {
            try
            {
                LogInfo("Service stopping...");
                _cts?.Cancel();
                Task.WaitAll(new[] { _workerFlush, _workerWrites }, TimeSpan.FromSeconds(5));
                _opc?.Dispose();
                LogInfo("Service stopped.");
            }
            catch (Exception ex)
            {
                LogError("OnStop", ex);
            }
        }

        private void LoadConfigurationAndTags()
        {
            using (var db = NewContext())
            {
                var query =
                    from g in db.rTagGroup.AsNoTracking()
                    where g.WindowsService == _serviceName && g.TagId != null
                    join t in db.Tag.AsNoTracking() on g.TagId equals t.Id
                    join p in db.Plc.AsNoTracking() on t.PlcId equals p.Id
                    join loc in db.Location.AsNoTracking() on p.Id equals loc.Id
                    select new
                    {
                        TagId = t.Id,
                        TagName = t.Name,
                        Topic = loc.Name,
                        g.OpcServer,
                        g.AddrOpcServer,
                        g.Rate
                    };

                var rows = query.ToList();
                if (rows.Count == 0)
                    throw new InvalidOperationException($"No Tags configured for WindowsService='{_serviceName}' in rTagGroup.");

                _opcServerProgId = rows.Select(r => r.OpcServer).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)) ?? "RSLinx OPC Server";
                _opcServerHost = rows.Select(r => r.AddrOpcServer).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)) ?? ".";
                _opcUpdateRate = (int)Math.Max(100, rows.Select(r => r.Rate).DefaultIfEmpty(500).Min());
                _topicName = rows.Select(r => r.Topic).Distinct().Single();

                _myTagIds = rows.Select(r => r.TagId).ToHashSet();

                _tagIdToItemId = new Dictionary<long, string>();
                foreach (var r in rows)
                {
                    var topic = string.IsNullOrEmpty(_topicName) ? string.Empty : $"[{_topicName}]";
                    var sep = string.IsNullOrEmpty(_itemSeparator) ? string.Empty : _itemSeparator;
                    _tagIdToItemId[r.TagId] = $"{_itemPrefix}{topic}{sep}{r.TagName}{_itemSuffix}";
                }

                _opcItems = _tagIdToItemId.Values.Select(id => new Item { ItemName = id, Active = true }).ToArray();

                _tagIdToMeasureRowId = db.rInstrumentMeasure
                    .AsNoTracking()
                    .Where(m => _myTagIds.Contains(m.TagId))
                    .GroupBy(m => m.TagId)
                    .Select(g => g.OrderByDescending(x => x.LastDh).FirstOrDefault())
                    .Where(m => m != null)
                    .ToDictionary(m => m.TagId, m => m.Id);
            }
        }

        private async Task ConnectWithRetryAsync(CancellationToken ct)
        {
            var backoff = new Backoff(_reconnectInitialMs, _reconnectMaxMs);
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    LogInfo($"Connecting to OPC Server '{_opcServerHost}\\{_opcServerProgId}' (UpdateRate={_opcUpdateRate}ms)...");
                    await _opc.ConnectAsync(ct);
                    LogInfo("Connected. Creating subscription...");
                    await _opc.CreateOrResetSubscriptionAsync("GRP_" + _topicName, _opcItems, ct);
                    LogInfo("Subscription active.");
                    return;
                }
                catch (Exception ex)
                {
                    LogWarn($"Connect failed: {ex.Message}. Will retry...");
                    await backoff.DelayAsync(ct);
                }
            }
        }

        private void HandleDisconnected(string reason)
        {
            LogWarn("OPC disconnected: " + reason);
            _ = Task.Run(async () =>
            {
                try { await _opc.DisposeAsync(); } catch { }
                if (_cts?.IsCancellationRequested == false)
                    await ConnectWithRetryAsync(_cts.Token);
            });
        }

        private void HandleDataChanged(ItemValueResult[] changes)
        {
            var now = DateTime.UtcNow;
            foreach (var c in changes ?? Array.Empty<ItemValueResult>())
            {
                if (c?.ResultID == null || !c.ResultID.Succeeded) continue;
                var itemId = c.ItemName ?? string.Empty;
                var tagId = _tagIdToItemId.FirstOrDefault(kv => kv.Value.Equals(itemId, StringComparison.OrdinalIgnoreCase)).Key;
                if (tagId == 0) continue;
                var value = c.Value?.ToString();
                _qMeasures.Enqueue((tagId, value, now));
            }
        }

        private async Task FlushMeasuresLoopAsync(CancellationToken ct)
        {
            var interval = TimeSpan.FromMilliseconds(_flushMeasuresMs);
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(interval, ct);

                    var dict = new Dictionary<long, (string Value, DateTime Ts)>();
                    while (_qMeasures.TryDequeue(out var it))
                        dict[it.TagId] = (it.Value, it.Ts);

                    if (dict.Count == 0) continue;

                    var toUpdate = dict.Where(kv => _tagIdToMeasureRowId.ContainsKey(kv.Key)).ToList();
                    if (toUpdate.Count == 0) continue;

                    using (var db = NewContext())
                    {
                        var ids = toUpdate.Select(kv => _tagIdToMeasureRowId[kv.Key]).ToList();
                        var rows = db.rInstrumentMeasure.Where(m => ids.Contains(m.Id)).ToList();
                        foreach (var row in rows)
                        {
                            var kv = toUpdate.FirstOrDefault(p => _tagIdToMeasureRowId[p.Key] == row.Id);
                            if (kv.Key == 0) continue;
                            row.Value = kv.Value.Value;
                            row.dh = kv.Value.Ts;
                            row.LastDh = row.dh;
                            row.ErrorCode = string.Empty;
                            row.ErrorString = string.Empty;
                            row.isUpdating = false;
                        }
                        db.SaveChanges();
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    LogError("FlushMeasuresLoopAsync", ex);
                }
            }
        }

        private async Task PollWritesLoopAsync(CancellationToken ct)
        {
            var interval = TimeSpan.FromMilliseconds(_pollWritesMs);
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(interval, ct);
                    if (!_opc.IsConnected) continue;

                    List<rTagWrite> pending;
                    using (var db = NewContext())
                    {
                        pending = db.rTagWrite
                                    .AsNoTracking()
                                    .Include(x => x.Tag)
                                    .Where(x => x.Write == true && x.Tag != null && _myTagIds.Contains(x.Tag.Id))
                                    .ToList();
                    }

                    if (pending.Count == 0) continue;

                    var writeItems = new List<(string ItemId, object Value, long TagId)>();
                    foreach (var w in pending)
                    {
                        if (w.Tag == null) continue;
                        if (!_tagIdToItemId.TryGetValue(w.Tag.Id, out var itemId)) continue;
                        writeItems.Add((itemId, (object)w.Value, w.Tag.Id));
                    }
                    if (writeItems.Count == 0) continue;

                    var itemIds = writeItems.Select(i => i.ItemId).ToArray();
                    var values = writeItems.Select(i => i.Value).ToArray();
                    var results = await _opc.WriteAsync(itemIds, values, ct);

                    var okTagIds = new HashSet<long>(
                        writeItems.Where((w, idx) => idx < results.Length && results[idx].ResultID?.Succeeded == true)
                                  .Select(w => w.TagId));

                    if (okTagIds.Count > 0)
                    {
                        using (var db = NewContext())
                        {
                            var toClear = db.rTagWrite
                                            .Where(x => x.Write == true && x.Tag != null && okTagIds.Contains(x.Tag.Id))
                                            .ToList();
                            foreach (var x in toClear) x.Write = false;
                            db.SaveChanges();
                        }
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    LogError("PollWritesLoopAsync", ex);
                }
            }
        }

        private static WriteReadContext NewContext()
        {
            var db = new WriteReadContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = true;
            return db;
        }

        private void LogInfo(string msg) => SaveLog("INFO", msg);
        private void LogWarn(string msg) => SaveLog("WARN", msg);
        private void LogError(string where, Exception ex) => SaveLog("ERROR", $"{where}: {ex.Message} | {ex.StackTrace}");

        private void SaveLog(string level, string msg)
        {
            try
            {
                using (var db = NewContext())
                {
                    db.Log.Add(new Log
                    {
                        Id = Guid.NewGuid(),
                        ApplicationId = 0,
                        User = Environment.MachineName,
                        dh = DateTime.Now,
                        Message = $"[{level}] {_serviceName} - {msg}",
                        LocationId = 0
                    });
                    db.SaveChanges();
                }
            }
            catch { }
        }

        private sealed class Backoff
        {
            private readonly int _initial, _max;
            private int _cur;
            public Backoff(int initialMs, int maxMs)
            {
                _initial = Math.Max(500, initialMs);
                _max = Math.Max(_initial, maxMs);
                _cur = _initial;
            }
            public async Task DelayAsync(CancellationToken ct)
            {
                await Task.Delay(_cur, ct);
                _cur = Math.Min(_cur * 2, _max);
            }
        }
    }
}
