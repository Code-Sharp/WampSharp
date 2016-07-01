using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WampSharp.AspNetCore.WebSockets.Server
{
    public class WebSocketData
    {
        internal WebSocketData(WebSocket webSocket)
        {
            WebSocket = webSocket;
        }

        public WebSocket WebSocket { get; }

        public Task ReadTask { get; internal set; }
    }
}