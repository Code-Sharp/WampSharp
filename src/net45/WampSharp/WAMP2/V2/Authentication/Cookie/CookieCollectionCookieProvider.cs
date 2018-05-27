using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An implementation of <see cref="ICookieProvider"/> based on <see cref="CookieCollection"/>.
    /// </summary>
    public class CookieCollectionCookieProvider : ICookieProvider
    {
        private readonly CookieCollection mCookieCollection;

        /// <summary>
        /// Creates a new instance of <see cref="CookieCollectionCookieProvider"/> given the underlying
        /// <see cref="CookieCollection"/>.
        /// </summary>
        /// <param name="cookieCollection">The underlying <see cref="CookieCollection"/>.</param>
        public CookieCollectionCookieProvider(CookieCollection cookieCollection)
        {
            mCookieCollection = cookieCollection;
        }

        /// <summary>
        /// <see cref="ICookieProvider.Cookies"/>
        /// </summary>
        public IEnumerable<Cookie> Cookies => mCookieCollection.Cast<Cookie>();

        /// <summary>
        /// <see cref="ICookieProvider.GetCookieByName"/>
        /// </summary>
        public Cookie GetCookieByName(string cookieName)
        {
            return mCookieCollection[cookieName];
        }
    }
}