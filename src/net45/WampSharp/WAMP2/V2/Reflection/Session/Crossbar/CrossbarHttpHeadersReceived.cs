using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class CrossbarHttpHeadersReceived
    {
        [PropertyName("origin")]
        public string Origin { get; set; }

        [PropertyName("upgrade")]
        public string Upgrade { get; set; }

        [PropertyName("accept-language")]
        public string AcceptLanguage { get; set; }

        [PropertyName("accept-encoding")]
        public string AcceptEncoding { get; set; }

        [PropertyName("sec-websocket-version")]
        public string SecWebsocketVersion { get; set; }

        [PropertyName("sec-websocket-protocol")]
        public string SecWebsocketProtocol { get; set; }

        [PropertyName("host")]
        public string Host { get; set; }

        [PropertyName("sec-websocket-key")]
        public string SecWebsocketKey { get; set; }

        [PropertyName("user-agent")]
        public string UserAgent { get; set; }

        [PropertyName("connection")]
        public string Connection { get; set; }

        [PropertyName("pragma")]
        public string Pragma { get; set; }

        [PropertyName("cache-control")]
        public string CacheControl { get; set; }

        [PropertyName("sec-websocket-extensions")]
        public string SecWebsocketExtensions { get; set; }
    }
}