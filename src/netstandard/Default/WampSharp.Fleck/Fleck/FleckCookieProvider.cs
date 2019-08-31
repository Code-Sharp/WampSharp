using System.Collections.Generic;
using System.Net;
using Fleck;
using WampSharp.V2.Authentication;

namespace WampSharp.Fleck
{
    internal class FleckCookieProvider : CookieCollectionCookieProvider
    {
        public FleckCookieProvider(IWebSocketConnectionInfo connectionInfo) :
            base(GetCookieCollection(connectionInfo))
        {
        }

        private static CookieCollection GetCookieCollection(IWebSocketConnectionInfo connectionInfo)
        {
            CookieCollection result = new CookieCollection();

            foreach (KeyValuePair<string, string> cookie in connectionInfo.Cookies)
            {
                result.Add(new Cookie(cookie.Key, cookie.Value));
            }

            return result;
        }
    }
}