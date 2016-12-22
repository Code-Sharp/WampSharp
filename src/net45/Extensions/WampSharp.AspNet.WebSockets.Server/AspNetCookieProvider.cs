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

            foreach (HttpCookie cookie in httpContext.Request.Cookies)
            {
                result.Add(new Cookie(cookie.Name, cookie.Value));
            }

            return result;
        }
    }
}