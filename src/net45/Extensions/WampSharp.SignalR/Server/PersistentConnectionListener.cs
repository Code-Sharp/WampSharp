using Microsoft.AspNet.SignalR;

namespace WampSharp.SignalR
{
    internal class PersistentConnectionListener : PersistentConnection
    {
        private readonly ISignalRConnectionListenerAdapter mAdapter;

        public PersistentConnectionListener(ISignalRConnectionListenerAdapter adapter)
        {
            mAdapter = adapter;
        }

        protected override System.Threading.Tasks.Task OnConnected(IRequest request, string connectionId)
        {
            mAdapter.OnConnected(connectionId, this.Connection);

            return base.OnConnected(request, connectionId);
        }

        protected override System.Threading.Tasks.Task OnReceived(IRequest request, string connectionId, string data)
        {
            mAdapter.OnReceived(connectionId, data);

            return base.OnReceived(request, connectionId, data);
        }

        protected override System.Threading.Tasks.Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            mAdapter.OnDisconnected(connectionId);

            return base.OnDisconnected(request, connectionId, stopCalled);
        }
    }
}