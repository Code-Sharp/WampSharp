using Microsoft.AspNet.SignalR;

namespace WampSharp.SignalR
{
    internal interface ISignalRConnectionListenerAdapter
    {
        void OnConnected(string connectionId, IConnection connection);
        void OnReceived(string connectionId, string data);
        void OnDisconnected(string connectionId);
    }
}