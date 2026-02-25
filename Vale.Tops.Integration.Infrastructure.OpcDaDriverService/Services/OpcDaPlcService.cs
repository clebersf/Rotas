using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using OPCAutomation; // COM: OPC Automation 2.0
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager; // WriteReadContext

namespace Vale.Tops.Integration.OpcDaDriver.Services
{
    public class OpcDaPlcService : ServiceBase
    {
        // Config
        private readonly string _serviceName;
        private readonly int _flushMeasuresMs;
        private readonly int _pollWritesMs;
        private readonly int _reconnectInitialMs;
        private readonly int _reconnectMaxMs;
        private readonly string _itemPrefix;
        private readonly string _itemSeparator;
        private readonly string _itemSuffix;

        // NOVO: ID do PLC raiz (rtaggroup)
        private readonly int _plcId;

        // Infra
        private CancellationTokenSource _cts;
        private Task _workerFlush;
        private Task _workerWrites;

        // OPC (Automation)
        private Thread _opcStaThread;
        private ManualResetEvent _opcReady = new ManualResetEvent(false);
        private volatile bool _opcLoopExit = false;
        private OPCServer _server;
        private OPCGroups _groups;
        private OPCGroup _group;
        private OPCItems _items;

        // Mapeamentos
        private Dictionary<long, string> _tagIdToItemId;
        private Dictionary<string, long> _itemIdToTagId; // reverso
        private Dictionary<long, int> _tagIdToServerHandle;
        private Dictionary<long, long> _tagIdToMeasureRowId;
        private HashSet<long> _myTagIds;

        // Param OPC
        private string _opcServerProgId;
        private string _opcServerHost;
        private int _opcUpdateRate;
        private string _topicName;

        // Fila de medidas
        private readonly ConcurrentQueue<Tuple<long, string, DateTime>> _qMeasures =
            new ConcurrentQueue<Tuple<long, string, DateTime>>();

        public OpcDaPlcService(string serviceName)
        {
            _serviceName = string.IsNullOrWhiteSpace(serviceName) ? (ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver") : serviceName;
            ServiceName = _serviceName;

            _flushMeasuresMs = ParseInt("Timers.FlushMeasuresMs", 300);
            _pollWritesMs = ParseInt("Timers.PollWritesMs", 500);
            _reconnectInitialMs = ParseInt("Timers.ReconnectInitialMs", 2000);
            _reconnectMaxMs = ParseInt("Timers.ReconnectMaxMs", 60000);
            _itemPrefix = ConfigurationManager.AppSettings["ItemId.Prefix"] ?? string.Empty;
            _itemSeparator = ConfigurationManager.AppSettings["ItemId.Separator"] ?? ".";
            _itemSuffix = ConfigurationManager.AppSettings["ItemId.Suffix"] ?? string.Empty;

            // NOVO
            _plcId = ParseInt("PLC.Id", 0);
        }

#if DEBUG
        public void DebugRun()
        {
            OnStart(null);
            Console.WriteLine("[" + _serviceName + "] DEBUG mode. Pressione ENTER para parar...");
            Console.ReadLine();
            OnStop();
        }
#endif

        protected override void OnStart(string[] args)
        {
            _cts = new CancellationTokenSource();
            LogInfo("Service starting...");

            if (_plcId <= 0)
                throw new InvalidOperationException("Configuração inválida: 'PLC.Id' não informado ou <= 0 no App.config.");

            LoadConfigurationAndTags(); // usa _plcId (PLC raiz)  [1](https://globalvale-my.sharepoint.com/personal/cleber_ferreira_vale_com/Documents/Microsoft%20Copilot%20Chat%20Files/OpcDaPlcService.cs)

            _opcStaThread = new Thread(OPCLoopSTA);
            _opcStaThread.Name = "OPC STA Thread - " + _serviceName;
            _opcStaThread.IsBackground = true;
            _opcStaThread.SetApartmentState(ApartmentState.STA);
            _opcStaThread.Start();

            WaitHandle.WaitAny(new WaitHandle[] { _opcReady, _cts.Token.WaitHandle }, 15000);

            _workerFlush = Task.Run(() => FlushMeasuresLoopAsync(_cts.Token));
            _workerWrites = Task.Run(() => PollWritesLoopAsync(_cts.Token));

            LogInfo("Service started. Subscribing " + _myTagIds.Count + " tags on topic '" + _topicName + "'.");
        }

        protected override void OnStop()
        {
            try
            {
                LogInfo("Service stopping...");
                if (_cts != null) _cts.Cancel();
                try { Task.WaitAll(new[] { _workerFlush, _workerWrites }, TimeSpan.FromSeconds(5)); } catch { }
                _opcLoopExit = true;
                if (_opcStaThread != null && _opcStaThread.IsAlive)
                    _opcStaThread.Join(TimeSpan.FromSeconds(5));
                LogInfo("Service stopped.");
            }
            catch (Exception ex)
            {
                LogError("OnStop", ex);
            }
        }

        // =============================== OPC LOOP (STA) ===============================
        private void OPCLoopSTA()
        {
            var backoff = new Backoff(_reconnectInitialMs, _reconnectMaxMs);

            while (!_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested))
            {
                try
                {
                    EnsureDisconnected();
                    _server = new OPCServer();
                    _server.Connect(_opcServerProgId, _opcServerHost);
                    _groups = _server.OPCGroups;
                    _group = _groups.Add("GRP_" + _topicName);
                    _group.UpdateRate = _opcUpdateRate;
                    _group.DeadBand = 0;
                    _group.IsActive = true;
                    _group.IsSubscribed = true;
                    _group.DataChange += Group_DataChange;

                    _items = _group.OPCItems;

                    var itemIds = Create1Based(_tagIdToItemId.Values.ToArray());
                    var clientHandle = Create1Based(_tagIdToItemId.Keys.Select(k => (int)k).ToArray());
                    Array serverHandles; Array errors;
                    _items.AddItems(_tagIdToItemId.Count, ref itemIds, ref clientHandle, out serverHandles, out errors);

                    _tagIdToServerHandle = new Dictionary<long, int>(_tagIdToItemId.Count);
                    for (int i = 1; i <= _tagIdToItemId.Count; i++)
                    {
                        int err = Convert.ToInt32(errors.GetValue(i));
                        if (err == 0)
                        {
                            int sh = Convert.ToInt32(serverHandles.GetValue(i));
                            long tid = Convert.ToInt64(clientHandle.GetValue(i));
                            _tagIdToServerHandle[tid] = sh;
                        }
                    }

                    _opcReady.Set();
                    LogInfo("OPC connected and subscribed.");

                    while (!_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested))
                        Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    LogWarn("OPC loop exception: " + ex.Message + ". Will try reconnect...");
                    try { EnsureDisconnected(); } catch { }
                    int ms = backoff.Next();
                    var endAt = DateTime.UtcNow.AddMilliseconds(ms);
                    while (DateTime.UtcNow < endAt && !_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested))
                        Thread.Sleep(200);
                }
                finally
                {
                    try { if (_group != null) _group.DataChange -= Group_DataChange; } catch { }
                    EnsureDisconnected();
                }
            }
        }

        private void EnsureDisconnected()
        {
            try { if (_items != null) _items = null; } catch { }
            try
            {
                if (_group != null)
                {
                    try { if (_groups != null) _groups.Remove(_group.Name); } catch { }
                    _group = null;
                }
            }
            catch { }
            try { if (_groups != null) _groups = null; } catch { }
            try { if (_server != null) { _server.Disconnect(); _server = null; } } catch { }
        }

        // =============================== DataChange ===============================
        private void Group_DataChange(int TxnId, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            var now = DateTime.UtcNow;
            for (int i = 1; i <= NumItems; i++)
            {
                int q = Convert.ToInt32(Qualities.GetValue(i));
                if ((q & 0xC0) != 0xC0) continue; // qualidade boa
                long tagId = Convert.ToInt64(ClientHandles.GetValue(i));
                string value = null; var v = ItemValues.GetValue(i); if (v != null) value = v.ToString();
                _qMeasures.Enqueue(Tuple.Create(tagId, value, now));
            }
        }

        // =============================== Persistência ===============================
        private async Task FlushMeasuresLoopAsync(CancellationToken ct)
        {
            var interval = TimeSpan.FromMilliseconds(_flushMeasuresMs);
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(interval, ct);

                    var latest = new Dictionary<long, Tuple<string, DateTime>>();
                    Tuple<long, string, DateTime> item;
                    while (_qMeasures.TryDequeue(out item))
                        latest[item.Item1] = Tuple.Create(item.Item2, item.Item3);

                    if (latest.Count == 0) continue;

                    var toUpdate = new List<KeyValuePair<long, Tuple<string, DateTime>>>();
                    foreach (var kv in latest)
                        if (_tagIdToMeasureRowId.ContainsKey(kv.Key)) toUpdate.Add(kv);

                    if (toUpdate.Count == 0) continue;

                    using (var db = NewContext())
                    {
                        var ids = toUpdate.Select(kv => _tagIdToMeasureRowId[kv.Key]).ToList();
                        var rows = db.rInstrumentMeasure.Where(m => ids.Contains(m.Id)).ToList();

                        foreach (var row in rows)
                        {
                            long tagIdFound = 0; Tuple<string, DateTime> dataFound = null;
                            foreach (var kv in toUpdate)
                                if (_tagIdToMeasureRowId[kv.Key] == row.Id) { tagIdFound = kv.Key; dataFound = kv.Value; break; }
                            if (tagIdFound == 0 || dataFound == null) continue;

                            row.Value = dataFound.Item1;
                            row.dh = dataFound.Item2;
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

        // =============================== Escrita ===============================
        private async Task PollWritesLoopAsync(CancellationToken ct)
        {
            var interval = TimeSpan.FromMilliseconds(_pollWritesMs);
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(interval, ct);

                    if (_group == null || _items == null) continue;

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

                    var serverHandles = new List<int>();
                    var values = new List<object>();
                    var backMap = new List<long>();

                    foreach (var w in pending)
                    {
                        if (w.Tag == null) continue;
                        int sh;
                        if (!_tagIdToServerHandle.TryGetValue(w.Tag.Id, out sh)) continue;
                        serverHandles.Add(sh); values.Add((object)w.Value); backMap.Add(w.Tag.Id);
                    }

                    if (serverHandles.Count == 0) continue;

                    Array shArr = Create1Based(serverHandles.ToArray());
                    Array vlArr = Create1Based(values.ToArray());
                    Array errors;

                    // FIX: a escrita é via OPCGroup.SyncWrite, não OPCItems.Write
                    _group.SyncWrite(serverHandles.Count, ref shArr, ref vlArr, out errors);  // <<<<<<<<<<<<<<<<

                    var okTagIds = new HashSet<long>();
                    for (int i = 1; i <= serverHandles.Count; i++)
                    {
                        int err = Convert.ToInt32(errors.GetValue(i));
                        if (err == 0) okTagIds.Add(backMap[i - 1]);
                    }

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
                    try { EnsureDisconnected(); } catch { }
                }
            }
        }

        // =============================== Carregamento (EF) ===============================
        private void LoadConfigurationAndTags()
        {
            using (var db = NewContext())
            {
                // 1) Ler o PLC raiz (id = _plcId) em rTagGroup e extrair config OPC  [1](https://globalvale-my.sharepoint.com/personal/cleber_ferreira_vale_com/Documents/Microsoft%20Copilot%20Chat%20Files/OpcDaPlcService.cs)
                var plcRoot = db.rTagGroup
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == _plcId);

                if (plcRoot == null)
                    throw new InvalidOperationException("PLC id=" + _plcId + " não encontrado em rTagGroup.");

                // (Opcional) Validar se é nó raiz (ParentId == null) — tolerante a falta da propriedade
                var isRoot = true;
                try
                {
                    var parentIdProp = plcRoot.GetType().GetProperty("ParentId");
                    if (parentIdProp != null)
                    {
                        var parentVal = parentIdProp.GetValue(plcRoot);
                        isRoot = (parentVal == null);
                    }
                }
                catch { }

                if (!isRoot)
                {
                    LogWarn("Aviso: o registro id=" + _plcId + " em rTagGroup não parece ser nó raiz (ParentId != NULL).");
                }

                // Config OPC a partir do nó do PLC
                _opcServerProgId = !string.IsNullOrWhiteSpace(plcRoot.OpcServer) ? plcRoot.OpcServer : "RSLinx OPC Server";
                _opcServerHost = !string.IsNullOrWhiteSpace(plcRoot.AddrOpcServer) ? plcRoot.AddrOpcServer : ".";
                var rate = 0;
                try { rate = Convert.ToInt32(plcRoot.Rate); } catch { rate = 0; }
                _opcUpdateRate = (int)Math.Max(100, rate > 0 ? rate : 500);

                // 2) Trazer as TAGS filhas diretas do PLC (g.ParentId == _plcId), filtrando por serviço  [1](https://globalvale-my.sharepoint.com/personal/cleber_ferreira_vale_com/Documents/Microsoft%20Copilot%20Chat%20Files/OpcDaPlcService.cs)
                var query =
                    from g in db.rTagGroup.AsNoTracking()
                    where g.WindowsService == _serviceName
                          && g.TagId != null
                          && g.ParentId == _plcId // ajuste se a propriedade no EF tiver outro nome
                    join t in db.Tag.AsNoTracking() on g.TagId equals t.Id
                    join p in db.Plc.AsNoTracking() on t.PlcId equals p.Id
                    join loc in db.Location.AsNoTracking() on p.Id equals loc.Id
                    select new
                    {
                        TagId = t.Id,
                        TagName = t.Name,
                        Topic = loc.Name
                    };

                var rows = query.ToList();

                if (rows.Count == 0)
                    throw new InvalidOperationException("Nenhuma Tag filha do PLC id=" + _plcId + " configurada para WindowsService='" + _serviceName + "' em rTagGroup.");

                _topicName = rows.Select(r => r.Topic).Distinct().Single();

                _myTagIds = new HashSet<long>(rows.Select(r => r.TagId));
                _tagIdToItemId = new Dictionary<long, string>(rows.Count);
                _itemIdToTagId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

                foreach (var r in rows)
                {
                    var topic = string.IsNullOrEmpty(_topicName) ? string.Empty : "[" + _topicName + "]";
                    var sep = string.IsNullOrEmpty(_itemSeparator) ? string.Empty : _itemSeparator;
                    var itemId = _itemPrefix + topic + sep + r.TagName + _itemSuffix;
                    _tagIdToItemId[r.TagId] = itemId;
                    _itemIdToTagId[itemId] = r.TagId;
                }

                var measRows = db.rInstrumentMeasure
                    .AsNoTracking()
                    .Where(m => _myTagIds.Contains(m.TagId))
                    .GroupBy(m => m.TagId)
                    .Select(gp => gp.OrderByDescending(x => x.LastDh).FirstOrDefault())
                    .Where(m => m != null)
                    .ToList();

                _tagIdToMeasureRowId = new Dictionary<long, long>(measRows.Count);
                foreach (var m in measRows) _tagIdToMeasureRowId[m.TagId] = m.Id;
            }
        }

        // =============================== EF/LOG/Helpers ===============================
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
        private void LogError(string where, Exception ex) => SaveLog("ERROR", where + ": " + ex.Message + " \n " + ex.StackTrace);

        private void SaveLog(string level, string msg)
        {
            try
            {
                using (var db = NewContext())
                {
                    var log = new Log();
                    log.Id = Guid.NewGuid();
                    log.ApplicationId = 0;
                    log.User = Environment.MachineName;
                    log.dh = DateTime.Now;
                    log.Message = "[" + level + "] " + _serviceName + " - " + msg;
                    log.LocationId = 0;
                    db.Log.Add(log);
                    db.SaveChanges();
                }
            }
            catch { }
        }

        private static int ParseInt(string key, int @default)
        {
            int v; return int.TryParse(ConfigurationManager.AppSettings[key], out v) ? v : @default;
        }

        private static Array Create1Based<T>(T[] src)
        {
            var arr = Array.CreateInstance(typeof(T), src.Length + 1);
            for (int i = 0; i < src.Length; i++) arr.SetValue(src[i], i + 1); return arr;
        }

        private sealed class Backoff
        {
            private readonly int _initial; private readonly int _max; private int _cur; private bool _first = true;
            public Backoff(int initialMs, int maxMs) { _initial = Math.Max(500, initialMs); _max = Math.Max(_initial, maxMs); _cur = _initial; } // FIX
            public int Next() { if (_first) { _first = false; return _initial; } _cur = Math.Min(_cur * 2, _max); return _cur; }
        }
    }
}