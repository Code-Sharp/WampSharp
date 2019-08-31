using System.Collections.Generic;
using System.Net;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a mechanism that allows cookie read access for WebSocket based connections.
    /// </summary>
    public interface ICookieProvider
    {
        /// <summary>
        /// Gets all cookies present.
        /// </summary>
        IEnumerable<Cookie> Cookies { get; }

        /// <summary>
        /// Lookups a cookie by its name.
        /// </summary>
        /// <param name="cookieName">The given cookie name.</param>
        /// <returns>The requested cookie, or null if not present.</returns>
        Cookie GetCookieByName(string cookieName);
    }
}