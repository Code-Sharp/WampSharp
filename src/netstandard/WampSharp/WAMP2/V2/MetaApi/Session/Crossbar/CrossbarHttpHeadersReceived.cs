using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class CrossbarHttpHeadersReceived
    {
        [DataMember(Name = "origin")]
        public string Origin { get; set; }

        [DataMember(Name = "upgrade")]
        public string Upgrade { get; set; }

        [DataMember(Name = "accept-language")]
        public string AcceptLanguage { get; set; }

        [DataMember(Name = "accept-encoding")]
        public string AcceptEncoding { get; set; }

        [DataMember(Name = "sec-websocket-version")]
        public string SecWebsocketVersion { get; set; }

        [DataMember(Name = "sec-websocket-protocol")]
        public string SecWebsocketProtocol { get; set; }

        [DataMember(Name = "host")]
        public string Host { get; set; }

        [DataMember(Name = "sec-websocket-key")]
        public string SecWebsocketKey { get; set; }

        [DataMember(Name = "user-agent")]
        public string UserAgent { get; set; }

        [DataMember(Name = "connection")]
        public string Connection { get; set; }

        [DataMember(Name = "pragma")]
        public string Pragma { get; set; }

        [DataMember(Name = "cache-control")]
        public string CacheControl { get; set; }

        [DataMember(Name = "sec-websocket-extensions")]
        public string SecWebsocketExtensions { get; set; }
    }
}