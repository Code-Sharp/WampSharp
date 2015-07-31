using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WampSharp.V2.Authentication
{
    public class CookieCollectionCookieProvider : ICookieProvider
    {
        private readonly CookieCollection mCookieCollection;

        public CookieCollectionCookieProvider(CookieCollection cookieCollection)
        {
            mCookieCollection = cookieCollection;
        }

        public IEnumerable<Cookie> Cookies
        {
            get
            {
                return mCookieCollection.Cast<Cookie>();
            }
        }

        public Cookie GetCookieByName(string cookieName)
        {
            return mCookieCollection[cookieName];
        }
    }
}