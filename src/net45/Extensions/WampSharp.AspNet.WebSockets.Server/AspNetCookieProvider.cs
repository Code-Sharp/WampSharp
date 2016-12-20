using System.Collections.Generic;
using System.Net;
using System.Web;
using WampSharp.V2.Authentication;

namespace WampSharp.AspNet.WebSockets.Server
{
    internal class AspNetCookieProvider : CookieCollectionCookieProvider
    {
        public AspNetCookieProvider(HttpContext httpContext) :
            base(GetCookieCollection(httpContext))
        {
        }

        private static CookieCollection GetCookieCollection(HttpContext httpContext)
        {
            CookieCollection result = new CookieCollection();

            foreach (var cookie in httpContext.Request.Cookies)
            {
                if (cookie is KeyValuePair<string, string>)
                {
                    var castedCookie = (KeyValuePair<string, string>) cookie;
                    result.Add(new Cookie(castedCookie.Key, castedCookie.Value));
                }
            }

            return result;
        }
    }
}