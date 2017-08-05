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

            HttpCookieCollection cookies = httpContext.Request.Cookies;

            for (int i = 0; i < cookies.Count; i++)
            {
                HttpCookie cookie = cookies.Get(i);

                if (cookie != null)
                {
                    result.Add(new Cookie(cookie.Name, cookie.Value));
                }
            }

            return result;
        }
    }
}