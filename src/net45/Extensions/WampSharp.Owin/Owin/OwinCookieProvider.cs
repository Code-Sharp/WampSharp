using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Owin;
using WampSharp.V2.Authentication;

namespace WampSharp.Owin
{
    internal class OwinCookieProvider : ICookieProvider
    {
        private readonly IOwinContext mOwinContext;

        public OwinCookieProvider(IOwinContext owinContext)
        {
            mOwinContext = owinContext;
        }

        public IEnumerable<Cookie> Cookies
        {
            get
            {
                return mOwinContext.Request.Cookies.Select(x => new Cookie(x.Key, x.Value));
            }
        }

        public Cookie GetCookieByName(string cookieName)
        {
            string cookieValue = mOwinContext.Request.Cookies[cookieName];

            if (cookieValue != null)
            {
                return new Cookie(cookieName, cookieValue);
            }

            return null;
        }
    }
}