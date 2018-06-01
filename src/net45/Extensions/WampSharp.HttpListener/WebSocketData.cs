using System.Net.WebSockets;

namespace WampSharp.HttpListener
{
    public class WebSocketData
    {
        public HttpListenerWebSocketContext Context { get; }

        public string SubProtocol { get; }

        internal WebSocketData(HttpListenerWebSocketContext context, string subProtocol)
        {
            Context = context;
            SubProtocol = subProtocol;
        }
    }
}