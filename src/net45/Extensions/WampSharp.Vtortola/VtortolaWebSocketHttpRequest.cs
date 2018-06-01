using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using vtortola.WebSockets;

namespace WampSharp.Vtortola
{
    internal class VtortolaWebSocketHttpRequest
    {
        private readonly IDictionary<string, object> mItems;

        public VtortolaWebSocketHttpRequest(WebSocketHttpRequest request)
        {
            RequestUri = request.RequestUri;
            HttpVersion = request.HttpVersion.ToString();
            Cookies = request.Cookies;
            Headers = ExtractHeaders(request.Headers);
            WebSocketVersion = request.WebSocketVersion;
            WebSocketExtensions = 
                request.WebSocketExtensions.Select(x => x.Name).ToArray();
            mItems = request.Items;
        }

        private static IDictionary<string, string> ExtractHeaders(HttpHeadersCollection headers)
        {
            Dictionary<string, string> result =
                headers.HeaderNames.ToDictionary(x => x, x => headers[x]);

            return result;
        }

        public Uri RequestUri { get; }

        public string HttpVersion { get; }

        public CookieCollection Cookies { get; }

        public IDictionary<string, string> Headers { get; }

        public short WebSocketVersion { get; }

        public string[] WebSocketExtensions { get; }

        public IDictionary<string, object> Items => mItems;
    }
}