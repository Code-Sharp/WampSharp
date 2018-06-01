using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.WebSockets
{
    internal class ClientWebSocketWrapper : WebSocketWrapper, IClientWebSocketWrapper
    {
        private readonly ClientWebSocket mClientWebSocket;

        public ClientWebSocketWrapper(ClientWebSocket clientWebSocket) : 
            base(clientWebSocket)
        {
            mClientWebSocket = clientWebSocket;
        }

        public ClientWebSocketOptions Options => mClientWebSocket.Options;

        public Task ConnectAsync(Uri connectUri, CancellationToken cancellationToken)
        {
            return mClientWebSocket.ConnectAsync(connectUri, cancellationToken);
        }
    }
}