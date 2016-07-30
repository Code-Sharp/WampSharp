using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using WampSharp.V2.Authentication;

namespace WampSharp.AspNetCore.WebSockets.Server
{
    internal class AspNetCoreCookieProvider : CookieCollectionCookieProvider
    {
        public AspNetCoreCookieProvider(HttpContext httpContext) :
            base(GetCookieCollection(httpContext))
        {
        }

        private static CookieCollection GetCookieCollection(HttpContext httpContext)
        {
            CookieCollection result = new CookieCollection();

            foreach (KeyValuePair<string, string> cookie in httpContext.Request.Cookies)
            {
                result.Add(new Cookie(cookie.Key, cookie.Value));
            }

            return result;
        }
    }
}