using System.Net.WebSockets;
using WampSharp.V2.Authentication;

namespace WampSharp.HttpListener
{
    public class HttpListenerCookieProvider : CookieCollectionCookieProvider
    {
        public HttpListenerCookieProvider(HttpListenerWebSocketContext context) :
            base(context.CookieCollection)
        {
        }
    }
}