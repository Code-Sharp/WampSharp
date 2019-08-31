using System.Linq;
using System.Net;
using vtortola.WebSockets;
#if NETCORE
using HttpStatusCode = vtortola.WebSockets.HttpStatusCode;
#endif

namespace WampSharp.Vtortola
{
    internal class VtortolaWebSocketHttpResponse
    {
        private readonly string mWebSocketProtocol;

        public VtortolaWebSocketHttpResponse(WebSocketHttpResponse httpResponse)
        {
            WebSocketExtensions = httpResponse.WebSocketExtensions.Select(x => x.Name).ToArray();
            Cookies = httpResponse.Cookies;
            Status = httpResponse.Status;
            mWebSocketProtocol = httpResponse.WebSocketProtocol;
        }

        public CookieCollection Cookies { get; }

        public HttpStatusCode Status { get; }

        public string[] WebSocketExtensions { get; }

        public string WebSocketProtocol => mWebSocketProtocol;
    }
}