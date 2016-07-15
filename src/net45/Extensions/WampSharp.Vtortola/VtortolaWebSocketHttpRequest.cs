using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using vtortola.WebSockets;

namespace WampSharp.Vtortola
{
    internal class VtortolaWebSocketHttpRequest
    {
        private readonly Uri mRequestUri;
        private readonly string mHttpVersion;
        private readonly CookieCollection mCookies;
        private readonly IDictionary<string, string> mHeaders;
        private readonly short mWebSocketVersion;
        private readonly string[] mWebSocketExtensions;
        private readonly IDictionary<string, object> mItems;

        public VtortolaWebSocketHttpRequest(WebSocketHttpRequest request)
        {
            mRequestUri = request.RequestUri;
            mHttpVersion = request.HttpVersion.ToString();
            mCookies = request.Cookies;
            mHeaders = ExtractHeaders(request.Headers);
            mWebSocketVersion = request.WebSocketVersion;
            mWebSocketExtensions = 
                request.WebSocketExtensions.Select(x => x.Name).ToArray();
            mItems = request.Items;
        }

        private static IDictionary<string, string> ExtractHeaders(HttpHeadersCollection headers)
        {
            Dictionary<string, string> result =
                headers.AllKeys.ToDictionary(x => x, x => headers.Get(x));

            return result;
        }

        public Uri RequestUri
        {
            get { return mRequestUri; }
        }

        public string HttpVersion
        {
            get { return mHttpVersion; }
        }

        public CookieCollection Cookies
        {
            get { return mCookies; }
        }

        public IDictionary<string, string> Headers
        {
            get { return mHeaders; }
        }

        public short WebSocketVersion
        {
            get { return mWebSocketVersion; }
        }

        public string[] WebSocketExtensions
        {
            get { return mWebSocketExtensions; }
        }

        public IDictionary<string, object> Items
        {
            get { return mItems; }
        }
    }
}