using System.Linq;
using System.Net;
using vtortola.WebSockets;

namespace WampSharp.Vtortola
{
    internal class VtortolaWebSocketHttpResponse
    {
        private readonly string[] mWebSocketExtensions;
        private readonly CookieCollection mCookies;
        private readonly HttpStatusCode mStatus;
        private readonly string mWebSocketProtocol;

        public VtortolaWebSocketHttpResponse(WebSocketHttpResponse httpResponse)
        {
            mWebSocketExtensions = httpResponse.WebSocketExtensions.Select(x => x.Name).ToArray();
            mCookies = httpResponse.Cookies;
            mStatus = httpResponse.Status;
            mWebSocketProtocol = httpResponse.WebSocketProtocol;
        }

        public CookieCollection Cookies
        {
            get { return mCookies; }
        }

        public HttpStatusCode Status
        {
            get { return mStatus; }
        }

        public string[] WebSocketExtensions
        {
            get
            {
                return mWebSocketExtensions;
            }
        }

        public string WebSocketProtocol
        {
            get { return mWebSocketProtocol; }
        }
    }
}