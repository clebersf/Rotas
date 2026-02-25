using OPCAutomation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager;

namespace Vale.Tops.Integration.OpcDaDriver.Service
{
    public sealed class OpcStats
    {
        public bool IsConnected { get; set; }
        public int ItemsCount { get; set; }
        public long TotalUpdates { get; set; }
        public long UpdatesDelta { get; set; }
        public int QueueCount { get; set; }
        public DateTime? LastDataChangeUtc { get; set; }
        public DateTime? LastFlushUtc { get; set; }
        public string LastError { get; set; }
    }

    internal static class Win32Pump
    {
        [StructLayout(LayoutKind.Sequential)] internal struct POINT { public int x; public int y; }
        [StructLayout(LayoutKind.Sequential)] internal struct MSG { public IntPtr hwnd; public uint message; public UIntPtr wParam; public IntPtr lParam; public uint time; public POINT pt; }
        [DllImport("user32.dll")] internal static extern sbyte PeekMessage(out MSG msg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("user32.dll")] internal static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport("user32.dll")] internal static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        [DllImport("user32.dll")] internal static extern uint MsgWaitForMultipleObjects(uint nCount, IntPtr pHandles, bool bWaitAll, uint dwMilliseconds, uint dwWakeMask);
        internal const uint PM_REMOVE = 0x0001; internal const uint QS_ALLINPUT = 0x04FF;
    }

    public sealed class OpcDaPlcRunner : IDisposable
    {
        public Action<string> OnLog { get; }
        public Action<string> OnStatus { get; set; }
        public Action<(string ProgId, string Host, string Topic, int UpdateRate)> OnOpcConfig { get; set; }
        public Action<List<(long TagId, string TagName)>> OnTagsLoaded { get; set; }
        public Action<long, string, DateTime> OnValue { get; set; }

        private readonly string _serviceName; private readonly int _flushMeasuresMs; private readonly int _pollWritesMs; private readonly bool _writesEnabled; private readonly int _reconnectInitialMs; private readonly int _reconnectMaxMs; private readonly string _itemPrefix; private readonly string _itemSeparator; private readonly string _itemSuffix; private readonly bool _useBrackets; private readonly int _plcId;
        private CancellationTokenSource _cts; private Task _workerFlush; private Task _workerWrites; private Thread _opcStaThread; private ManualResetEvent _opcReady = new ManualResetEvent(false); private volatile bool _opcLoopExit = false; private OPCServer _server; private OPCGroups _groups; private OPCGroup _group; private OPCItems _items;
        private Dictionary<long, string> _tagIdToItemId; private Dictionary<string, long> _itemIdToTagId; private Dictionary<long, int> _tagIdToServerHandle; private Dictionary<long, long> _tagIdToMeasureRowId; private HashSet<long> _myTagIds; private string _opcServerProgId; private string _opcServerHost; private int _opcUpdateRate; private string _topicName;
        private readonly ConcurrentQueue<Tuple<long, string, DateTime>> _qMeasures = new ConcurrentQueue<Tuple<long, string, DateTime>>();
        // Stats
        private long _totalUpdates = 0; private long _updatesDelta = 0; private DateTime? _lastChangeUtc = null; private DateTime? _lastFlushUtc = null; private string _lastError = null; private volatile bool _isConnected = false; private int _itemsCount = 0;
        // OPC DA DataSource (valores oficiais do OPCAutomation)
        private const short OPC_DS_CACHE = 1;
        private const short OPC_DS_DEVICE = 2;
        public OpcDaPlcRunner(Action<string> logger)
        {
            OnLog = logger ?? (_ => { });
            _serviceName = ConfigurationManager.AppSettings["Service.Name"] ?? "Vale.OPCDA.Driver";
            _flushMeasuresMs = ParseInt("Timers.FlushMeasuresMs", 300);
            _pollWritesMs = ParseInt("Timers.PollWritesMs", 500);
            _writesEnabled = string.Equals(ConfigurationManager.AppSettings["Writes.Enabled"], "true", StringComparison.OrdinalIgnoreCase);
            _reconnectInitialMs = ParseInt("Timers.ReconnectInitialMs", 2000);
            _reconnectMaxMs = ParseInt("Timers.ReconnectMaxMs", 60000);
            _itemPrefix = ConfigurationManager.AppSettings["ItemId.Prefix"] ?? string.Empty;
            _itemSeparator = ConfigurationManager.AppSettings["ItemId.Separator"] ?? ".";
            _itemSuffix = ConfigurationManager.AppSettings["ItemId.Suffix"] ?? string.Empty;
            _useBrackets = string.Equals(ConfigurationManager.AppSettings["ItemId.UseBrackets"], "true", StringComparison.OrdinalIgnoreCase);
            _plcId = ParseInt("PLC.Id", 0);
        }

        public OpcStats SnapshotStatsAndResetDelta()
        {
            return new OpcStats
            {
                IsConnected = _isConnected,
                ItemsCount = _itemsCount,
                TotalUpdates = _totalUpdates,
                UpdatesDelta = Interlocked.Exchange(ref _updatesDelta, 0),
                QueueCount = _qMeasures.Count,
                LastDataChangeUtc = _lastChangeUtc,
                LastFlushUtc = _lastFlushUtc,
                LastError = _lastError
            };
        }

        public async Task StartAsync()
        {
            if (_plcId <= 0) throw new InvalidOperationException("App.config: 'PLC.Id' inválido.");
            _cts = new CancellationTokenSource(); OnStatus?.Invoke("Inicializando..."); OnLog("Loading configuration and tags...");
            try { var exeCfg = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; OnLog($"[DEBUG] ConfigFile: {exeCfg}"); var names = ConfigurationManager.ConnectionStrings.Cast<System.Configuration.ConnectionStringSettings>().Select(cs => cs.Name); OnLog("[DEBUG] ConnStrings: " + string.Join(", ", names)); } catch { }
            LoadConfigurationAndTags();
            _opcStaThread = new Thread(OPCLoopSTA) { IsBackground = true }; _opcStaThread.SetApartmentState(ApartmentState.STA); _opcStaThread.Start();
            WaitHandle.WaitAny(new WaitHandle[] { _opcReady, _cts.Token.WaitHandle }, 15000);
            _workerFlush = Task.Run(() => FlushMeasuresLoopAsync(_cts.Token));
            if (_writesEnabled) _workerWrites = Task.Run(() => PollWritesLoopAsync(_cts.Token)); else OnLog("Writes polling desabilitado (Writes.Enabled=false)");
            OnStatus?.Invoke("Rodando");
        }

        public async Task StopAsync()
        {
            OnStatus?.Invoke("Parando..."); _cts?.Cancel();
            try { if (_workerFlush != null) await Task.WhenAny(_workerFlush, Task.Delay(1500)); } catch { }
            try { if (_workerWrites != null) await Task.WhenAny(_workerWrites, Task.Delay(1500)); } catch { }
            _opcLoopExit = true; try { if (_opcStaThread != null && _opcStaThread.IsAlive) _opcStaThread.Join(1500); } catch { }
            EnsureDisconnected(); _opcReady.Reset(); OnStatus?.Invoke("Parado");
        }

        public async Task WriteTagAsync(long tagId, object value)
        {
            if (_group == null || _items == null) throw new InvalidOperationException("OPC não conectado.");
            if (!_tagIdToServerHandle.TryGetValue(tagId, out var sh)) throw new InvalidOperationException($"ServerHandle não encontrado para TagId={tagId}.");
            OPCItem it = _items.GetOPCItem(sh);
            if (!HasWriteAccess(it)) throw new InvalidOperationException($"Item '{_tagIdToItemId[tagId]}' sem direito de escrita (AccessRights={it.AccessRights}).");
            object typed; try { typed = CoerceToCanonicalVariant(it, Convert.ToString(value)); } catch (Exception ex) { throw new InvalidOperationException($"Valor inválido para tipo {((VarEnum)it.CanonicalDataType)}: {ex.Message}"); }
            Array shArr = Create1Based(new[] { sh }); Array vlArr = Create1Based(new[] { typed }); Array errors; _group.SyncWrite(1, ref shArr, ref vlArr, out errors); int err = Convert.ToInt32(errors.GetValue(1)); if (err != 0) throw new InvalidOperationException($"OPC SyncWrite falhou. Tag='{_tagIdToItemId[tagId]}', VT={((VarEnum)it.CanonicalDataType)}, HRESULT=0x{err:X8}");
            OnValue?.Invoke(tagId, typed?.ToString(), DateTime.UtcNow);
        }

        private void OPCLoopSTA()
        {
            var backoff = new Backoff(_reconnectInitialMs, _reconnectMaxMs);
            while (!_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested))
            {
                try
                {
                    EnsureDisconnected();
                    _server = new OPCServer(); _server.Connect(_opcServerProgId, _opcServerHost);
                    _groups = _server.OPCGroups; _group = _groups.Add("GRP_" + _topicName);
                    _group.UpdateRate = _opcUpdateRate; _group.DeadBand = 0; _group.IsActive = true; _group.IsSubscribed = true; _group.DataChange += Group_DataChange;
                    _items = _group.OPCItems;

                    var itemIds = Create1Based(_tagIdToItemId.Values.ToArray()); var clientHandle = Create1Based(_tagIdToItemId.Keys.Select(k => (int)k).ToArray());
                    Array serverHandles; Array errors; _items.AddItems(_tagIdToItemId.Count, ref itemIds, ref clientHandle, out serverHandles, out errors);
                    int errCount = 0; for (int i = 1; i <= _tagIdToItemId.Count; i++) { int er = Convert.ToInt32(errors.GetValue(i)); string id = (string)itemIds.GetValue(i); if (er != 0) { errCount++; OnLog($"AddItems: falha para '{id}', HRESULT=0x{er:X8}"); } }
                    OnLog($"AddItems: {_tagIdToItemId.Count - errCount} itens OK, {errCount} com erro.");

                    _tagIdToServerHandle = new Dictionary<long, int>(_tagIdToItemId.Count);
                    for (int i = 1; i <= _tagIdToItemId.Count; i++)
                    {
                        int er = Convert.ToInt32(errors.GetValue(i));
                        if (er == 0)
                        {
                            int sh = Convert.ToInt32(serverHandles.GetValue(i)); long tid = Convert.ToInt64(clientHandle.GetValue(i)); _tagIdToServerHandle[tid] = sh;
                            try { var it = _items.GetOPCItem(sh); it.IsActive = true; } catch (Exception ex) { OnLog($"WARN: não foi possível ativar item handle={sh}: {ex.Message}"); }
                        }
                    }

                    // ========================
                    // LEITURA FORÇADA INICIAL
                    // ========================
                    try
                    {
                        OnLog("[INFO] Realizando leitura inicial forçada (SyncRead)...");

                        int count = _tagIdToServerHandle.Count;
                        Array shArrAll = Array.CreateInstance(typeof(int), count + 1);
                        int idx = 1;
                        foreach (var kv in _tagIdToServerHandle)
                            shArrAll.SetValue(kv.Value, idx++);

                        Array values; Array readErrors; object qualitiesObj;
                        object timestampsObj;
                        _group.SyncRead(OPC_DS_DEVICE, count, ref shArrAll, out values, out readErrors, out qualitiesObj, out timestampsObj);

                        // Se preferir trabalhar como Array:
                        var qualities = (Array)qualitiesObj;
                        var timestamps = (Array)timestampsObj; 
                        var now = DateTime.UtcNow; idx = 1;
                        foreach (var kv in _tagIdToServerHandle)
                        {
                            long tagId = kv.Key;
                            int er = Convert.ToInt32(readErrors.GetValue(idx));
                            if (er == 0)
                            {
                                string value = values.GetValue(idx)?.ToString();
                                int q = Convert.ToInt32(qualities.GetValue(idx));
                                if ((q & 0xC0) == 0xC0)
                                {
                                    _qMeasures.Enqueue(Tuple.Create(tagId, value, now));
                                    Interlocked.Increment(ref _totalUpdates);
                                    Interlocked.Increment(ref _updatesDelta);
                                    _lastChangeUtc = now;
                                    OnValue?.Invoke(tagId, value, now);
                                }
                            }
                            else
                            {
                                OnLog($"[WARN] Leitura inicial: erro 0x{er:X8} no TagId={tagId}");
                            }
                            idx++;
                        }
                        OnLog("[INFO] Leitura inicial concluída. Valores enfileirados para flush.");
                    }
                    catch (Exception ex)
                    {
                        OnLog("[ERRO] Falha na leitura inicial: " + ex.Message);
                    }

                    _itemsCount = _tagIdToServerHandle.Count; _isConnected = true;
                    _opcReady.Set(); OnLog("OPC connected and subscribed."); OnStatus?.Invoke("Conectado");

                    while (!_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested))
                    {
                        Win32Pump.MsgWaitForMultipleObjects(0, IntPtr.Zero, false, 400, Win32Pump.QS_ALLINPUT);
                        Win32Pump.MSG msg; while (Win32Pump.PeekMessage(out msg, IntPtr.Zero, 0, 0, Win32Pump.PM_REMOVE) != 0) { Win32Pump.TranslateMessage(ref msg); Win32Pump.DispatchMessage(ref msg); }
                    }
                }
                catch (Exception ex)
                {
                    _lastError = ex.Message; _isConnected = false;
                    string hex = (ex is COMException com) ? $" (HRESULT=0x{com.ErrorCode:X8})" : "";
                    OnLog("OPC loop exception: " + ex.Message + hex + ". Reconnecting...");
                    try { EnsureDisconnected(); } catch { }
                    int ms = backoff.Next(); var endAt = DateTime.UtcNow.AddMilliseconds(ms);
                    while (DateTime.UtcNow < endAt && !_opcLoopExit && (_cts == null || !_cts.IsCancellationRequested)) Thread.Sleep(200);
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
            try { if (_group != null) { try { _groups?.Remove(_group.Name); } catch { } _group = null; } } catch { }
            try { if (_groups != null) _groups = null; } catch { }
            try { if (_server != null) { _server.Disconnect(); _server = null; } } catch { }
        }

        private void Group_DataChange(int TxnId, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            var now = DateTime.UtcNow;
            for (int i = 1; i <= NumItems; i++)
            {
                int q = Convert.ToInt32(Qualities.GetValue(i)); if ((q & 0xC0) != 0xC0) continue;
                long tagId = Convert.ToInt64(ClientHandles.GetValue(i)); string value = ItemValues.GetValue(i)?.ToString();
                _qMeasures.Enqueue(Tuple.Create(tagId, value, now));
                Interlocked.Increment(ref _totalUpdates); Interlocked.Increment(ref _updatesDelta); _lastChangeUtc = now;
                OnValue?.Invoke(tagId, value, now);
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
                    var latest = new Dictionary<long, Tuple<string, DateTime>>();
                    Tuple<long, string, DateTime> item; while (_qMeasures.TryDequeue(out item)) latest[item.Item1] = Tuple.Create(item.Item2, item.Item3);
                    if (latest.Count == 0) continue; OnLog($"Flush: {latest.Count} tags para persistir...");
                    var toUpdate = new List<KeyValuePair<long, Tuple<string, DateTime>>>();
                    foreach (var kv in latest) if (_tagIdToMeasureRowId.ContainsKey(kv.Key)) toUpdate.Add(kv);
                    if (toUpdate.Count == 0) continue;
                    using (var db = NewContext())
                    {
                        var ids = toUpdate.Select(kv => _tagIdToMeasureRowId[kv.Key]).ToList();
                        var rows = db.rInstrumentMeasure.Where(m => ids.Contains(m.Id)).ToList();
                        foreach (var row in rows)
                        {
                            long tagIdFound = 0; Tuple<string, DateTime> dataFound = null;
                            foreach (var kv in toUpdate) if (_tagIdToMeasureRowId[kv.Key] == row.Id) { tagIdFound = kv.Key; dataFound = kv.Value; break; }
                            if (tagIdFound == 0 || dataFound == null) continue;
                            row.Value = dataFound.Item1; row.dh = dataFound.Item2; row.LastDh = row.dh; row.ErrorCode = string.Empty; row.ErrorString = string.Empty; row.isUpdating = false;
                        }
                        db.SaveChanges();
                    }
                    _lastFlushUtc = DateTime.UtcNow;
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { _lastError = ex.Message; OnLog("FlushMeasuresLoopAsync: " + ex.Message); }
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
                    if (_group == null || _items == null) continue;
                    List<rTagWrite> pending;
                    using (var db = NewContext())
                    {
                        pending = db.rTagWrite.AsNoTracking().Include(x => x.Tag)
                            .Where(x => x.Write == true && x.Tag != null && _myTagIds.Contains(x.Tag.Id)).ToList();
                    }
                    if (pending.Count == 0) continue;
                    var serverHandles = new List<int>(); var values = new List<object>(); var backMap = new List<long>();
                    foreach (var w in pending)
                    {
                        if (w.Tag == null) continue;
                        if (!_tagIdToServerHandle.TryGetValue(w.Tag.Id, out var sh)) { OnLog($"Write skip TagId={w.Tag.Id}: sem server handle."); continue; }
                        OPCItem it; try { it = _items.GetOPCItem(sh); } catch { OnLog($"Write skip TagId={w.Tag.Id}: OPCItem não encontrado."); continue; }
                        if (!HasWriteAccess(it)) { OnLog($"Write skip TagId={w.Tag.Id}: item '{_tagIdToItemId[w.Tag.Id]}' sem direito de escrita (AccessRights={it.AccessRights})."); continue; }
                        object typed; try { typed = CoerceToCanonicalVariant(it, w.Value); } catch (Exception ex) { OnLog($"Write skip TagId={w.Tag.Id}: valor inválido para {((VarEnum)it.CanonicalDataType)} — {ex.Message}"); continue; }
                        serverHandles.Add(sh); values.Add(typed); backMap.Add(w.Tag.Id);
                    }
                    if (serverHandles.Count == 0) continue;
                    Array shArr = Create1Based(serverHandles.ToArray()); Array vlArr = Create1Based(values.ToArray()); Array errors;
                    _group.SyncWrite(serverHandles.Count, ref shArr, ref vlArr, out errors);
                    var ok = new HashSet<long>();
                    for (int i = 1; i <= serverHandles.Count; i++) { int err = Convert.ToInt32(errors.GetValue(i)); if (err == 0) ok.Add(backMap[i - 1]); else OnLog($"Write FAIL TagId={backMap[i - 1]}: HRESULT=0x{err:X8}"); }
                    if (ok.Count > 0)
                    {
                        using (var db = NewContext())
                        {
                            var toClear = db.rTagWrite.Where(x => x.Write == true && x.Tag != null && ok.Contains(x.Tag.Id)).ToList();
                            foreach (var x in toClear) x.Write = false; db.SaveChanges();
                        }
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    _lastError = ex.Message; OnLog("PollWritesLoopAsync: " + ex.Message); try { EnsureDisconnected(); } catch { }
                }
            }
        }

        private void LoadConfigurationAndTags()
        {
            using (var db = NewContext())
            {
                var plcRoot = db.rTagGroup.AsNoTracking().FirstOrDefault(x => x.Id == _plcId);
                if (plcRoot == null) throw new InvalidOperationException($"PLC id={_plcId} não encontrado em rTagGroup.");
                _opcServerProgId = !string.IsNullOrWhiteSpace(plcRoot.OpcServer) ? plcRoot.OpcServer : "Matrikon.OPC.Simulation.1";
                _opcServerHost = string.IsNullOrWhiteSpace(plcRoot.AddrOpcServer) || plcRoot.AddrOpcServer.Trim() == "-" ? "." : plcRoot.AddrOpcServer;
                int rate = 0; try { rate = Convert.ToInt32(plcRoot.Rate); } catch { }
                _opcUpdateRate = (int)Math.Max(100, rate > 0 ? rate : 500);
                var rows = (from g in db.rTagGroup.AsNoTracking()
                            where g.TagId != null && g.ParentId == _plcId
                            join t in db.Tag.AsNoTracking() on g.TagId equals t.Id
                            join p in db.Plc.AsNoTracking() on t.PlcId equals p.Id
                            join loc in db.Location.AsNoTracking() on p.Id equals loc.Id
                            select new { TagId = t.Id, TagName = t.Name, Topic = loc.Name }).ToList();
                if (rows.Count == 0) throw new InvalidOperationException($"Nenhuma Tag filha do PLC id={_plcId} para WindowsService='{_serviceName}'.");
                _topicName = rows.Select(r => r.Topic).Distinct().Single();
                _myTagIds = new HashSet<long>(rows.Select(r => r.TagId));
                _tagIdToItemId = new Dictionary<long, string>(_myTagIds.Count); _itemIdToTagId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                foreach (var r in rows)
                {
                    var topic = string.IsNullOrEmpty(_topicName) ? string.Empty : (_useBrackets ? "[" + _topicName + "]" : _topicName);
                    var sep = string.IsNullOrEmpty(_itemSeparator) ? string.Empty : _itemSeparator;
                    var itemId = _itemPrefix + topic + sep + r.TagName + _itemSuffix;
                    _tagIdToItemId[r.TagId] = itemId; _itemIdToTagId[itemId] = r.TagId;
                }
                var measRows = db.rInstrumentMeasure.AsNoTracking().Where(m => _myTagIds.Contains(m.TagId))
                    .GroupBy(m => m.TagId).Select(gp => gp.OrderByDescending(x => x.LastDh).FirstOrDefault())
                    .Where(m => m != null).ToList();
                _tagIdToMeasureRowId = new Dictionary<long, long>(measRows.Count);
                foreach (var m in measRows) _tagIdToMeasureRowId[m.TagId] = m.Id;
                OnOpcConfig?.Invoke((_opcServerProgId, _opcServerHost, _topicName, _opcUpdateRate));
                OnTagsLoaded?.Invoke(rows.Select(r => (r.TagId, r.TagName)).ToList());
            }
            OnLog($"OPC: ProgID={_opcServerProgId}, Host={_opcServerHost}, Topic={_topicName}, Rate={_opcUpdateRate}ms");
        }

        private static WriteReadContext NewContext() { var db = new WriteReadContext(); db.Configuration.LazyLoadingEnabled = false; db.Configuration.ProxyCreationEnabled = false; db.Configuration.AutoDetectChangesEnabled = true; return db; }
        private static int ParseInt(string key, int def) { int v; return int.TryParse(ConfigurationManager.AppSettings[key], out v) ? v : def; }
        private static Array Create1Based<T>(T[] src) { var arr = Array.CreateInstance(typeof(T), src.Length + 1); for (int i = 0; i < src.Length; i++) arr.SetValue(src[i], i + 1); return arr; }
        private sealed class Backoff { private readonly int _initial; private readonly int _max; private int _cur; private bool _first = true; public Backoff(int initialMs, int maxMs) { _initial = Math.Max(500, initialMs); _max = Math.Max(_initial, maxMs); _cur = _initial; } public int Next() { if (_first) { _first = false; return _initial; } _cur = Math.Min(_cur * 2, _max); return _cur; } }
        private static object CoerceToCanonicalVariant(OPCItem it, string raw)
        {
            var vt = (VarEnum)it.CanonicalDataType; var ci = System.Globalization.CultureInfo.InvariantCulture;
            switch (vt)
            {
                case VarEnum.VT_I1: return Convert.ToSByte(double.Parse(raw, ci));
                case VarEnum.VT_UI1: { var v = Convert.ToInt32(double.Parse(raw, ci)); if (v < byte.MinValue || v > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(raw), "Fora de faixa para UInt8 (0..255)."); return (byte)v; }
                case VarEnum.VT_I2: return Convert.ToInt16(double.Parse(raw, ci));
                case VarEnum.VT_UI2: { var v = Convert.ToInt64(double.Parse(raw, ci)); if (v < ushort.MinValue || v > ushort.MaxValue) throw new ArgumentOutOfRangeException(nameof(raw), "Fora de faixa para UInt16 (0..65535)."); return (ushort)v; }
                case VarEnum.VT_I4:
                case VarEnum.VT_INT: return Convert.ToInt32(double.Parse(raw, ci));
                case VarEnum.VT_UI4: { var v = Convert.ToDouble(raw, ci); if (v < uint.MinValue || v > uint.MaxValue) throw new ArgumentOutOfRangeException(nameof(raw), "Fora de faixa para UInt32."); return Convert.ToUInt32(v); }
                case VarEnum.VT_I8: return Convert.ToInt64(double.Parse(raw, ci));
                case VarEnum.VT_UI8: { var bi = System.Numerics.BigInteger.Parse(raw, ci); if (bi < ulong.MinValue || bi > ulong.MaxValue) throw new ArgumentOutOfRangeException(nameof(raw), "Fora de faixa para UInt64."); return (ulong)bi; }
                case VarEnum.VT_R4: return Convert.ToSingle(double.Parse(raw, ci));
                case VarEnum.VT_R8: return Convert.ToDouble(raw, ci);
                case VarEnum.VT_BOOL:
                    if (string.Equals(raw, "1") || string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase)) return true;
                    if (string.Equals(raw, "0") || string.Equals(raw, "false", StringComparison.OrdinalIgnoreCase)) return false;
                    throw new FormatException("Valor booleano inválido. Use 0/1 ou true/false.");
                case VarEnum.VT_BSTR: return raw ?? string.Empty;
                default: return raw;
            }
        }
        private static bool HasWriteAccess(OPCItem it) { try { int ar = it.AccessRights; return (ar & 0x02) == 0x02; } catch { return true; } }
        public void Dispose() { try { _cts?.Cancel(); } catch { } try { _workerFlush?.Dispose(); } catch { } try { _workerWrites?.Dispose(); } catch { } try { _cts?.Dispose(); } catch { } try { _opcReady?.Dispose(); } catch { } EnsureDisconnected(); }
    }
}
