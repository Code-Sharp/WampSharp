using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WampSharp.AspNetCore.WebSockets.Server
{
    public class WebSocketData
    {
        internal WebSocketData(WebSocket webSocket, HttpContext httpContext)
        {
            WebSocket = webSocket;
            HttpContext = httpContext;
        }

        public WebSocket WebSocket { get; }

        public HttpContext HttpContext { get; }

        public Task ReadTask { get; internal set; }
    }
}