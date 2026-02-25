
using Opc;
using Opc.Da;
using OpcCom;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Vale.Tops.Integration.OpcDaDriver.Opc
{
    public sealed class OpcDaClient : IAsyncDisposable, IDisposable
    {
        private readonly string _host;
        private readonly string _progId;
        private readonly int _updateRate;
        private readonly string _serviceName;

        private Server _server;
        private Subscription _sub;

        public event Action<ItemValueResult[]> OnDataChanged;
        public event Action<string> OnDisconnected;

        public bool IsConnected => _server != null && _server.IsConnected;

        public OpcDaClient(string host, string progId, int updateRateMs, string serviceName)
        {
            _host = string.IsNullOrWhiteSpace(host) ? "." : host;
            _progId = string.IsNullOrWhiteSpace(progId) ? "RSLinx OPC Server" : progId;
            _updateRate = Math.Max(100, updateRateMs);
            _serviceName = serviceName ?? "Vale.OPCDA.Driver";
        }

        public async Task ConnectAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                var url = new URL($"opcda://{_host}/{_progId}");
                _server = new Server(new Factory(), url);
                _server.Connect();
                _server.ServerShutdown += Server_ServerShutdown;
            }, ct);
        }

        private void Server_ServerShutdown(string reason)
        {
            OnDisconnected?.Invoke(reason ?? "Server shutdown");
        }

        public async Task CreateOrResetSubscriptionAsync(string groupName, Item[] items, CancellationToken ct)
        {
            await Task.Run(() =>
            {
                if (_sub != null)
                {
                    _sub.DataChanged -= Sub_DataChanged;
                    _server.CancelSubscription(_sub);
                    _sub.Dispose();
                    _sub = null;
                }

                var state = new SubscriptionState
                {
                    Name = groupName,
                    Active = true,
                    UpdateRate = _updateRate,
                    Deadband = 0
                };

                _sub = (Subscription)_server.CreateSubscription(state);
                _sub.DataChanged += Sub_DataChanged;

                var results = _sub.AddItems(items);
            }, ct);
        }

        private void Sub_DataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            try { OnDataChanged?.Invoke(values); } catch { }
        }

        public async Task<ItemValueResult[]> ReadAsync(string[] itemIds, CancellationToken ct)
        {
            if (!IsConnected || _sub == null) return Array.Empty<ItemValueResult>();
            return await Task.Run(() =>
            {
                var items = itemIds.Select(id => new Item { ItemName = id, Active = true }).ToArray();
                return _sub.Read(items);
            }, ct);
        }

        public async Task<IdentifiedResult[]> WriteAsync(string[] itemIds, object[] values, CancellationToken ct)
        {
            if (!IsConnected || _sub == null || itemIds.Length == 0) return Array.Empty<IdentifiedResult>();
            return await Task.Run(() =>
            {
                var itemValues = new ItemValue[itemIds.Length];
                for (int i = 0; i < itemIds.Length; i++)
                    itemValues[i] = new ItemValue { ItemName = itemIds[i], Value = values[i] };
                return _sub.Write(itemValues);
            }, ct);
        }

        public async ValueTask DisposeAsync() { await Task.Run(() => Dispose()); }

        public void Dispose()
        {
            try
            {
                if (_sub != null)
                {
                    _sub.DataChanged -= Sub_DataChanged;
                    _server?.CancelSubscription(_sub);
                    _sub.Dispose();
                    _sub = null;
                }
            }
            catch { }
            try
            {
                if (_server != null)
                {
                    _server.ServerShutdown -= Server_ServerShutdown;
                    _server.Disconnect();
                    _server.Dispose();
                    _server = null;
                }
            }
            catch { }
        }
    }
}
