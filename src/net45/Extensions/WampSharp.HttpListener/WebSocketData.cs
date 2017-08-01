using System.Net;
using System.Net.WebSockets;

namespace WampSharp.HttpListener
{
    public class WebSocketData
    {
        public HttpListenerWebSocketContext Context { get; private set; }

        public string SubProtocol { get; private set; }

        internal WebSocketData(HttpListenerWebSocketContext context, string subProtocol)
        {
            Context = context;
            SubProtocol = subProtocol;
        }
    }
}