using System.Threading;
using vtortola.WebSockets;

namespace WampSharp.Vtortola
{
    public class WebSocketData
    {
        public WebSocket WebSocket { get; }
        public CancellationToken CancellationToken { get; }

        public WebSocketData(WebSocket webSocket, CancellationToken cancellationToken)
        {
            WebSocket = webSocket;
            CancellationToken = cancellationToken;
        }
    }
}