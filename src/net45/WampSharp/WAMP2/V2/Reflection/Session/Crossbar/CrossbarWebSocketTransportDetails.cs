using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class CrossbarWebSocketTransportDetails : WampTransportDetails
    {
        [PropertyName("peer")]
        public string Peer { get; set; }

        [PropertyName("protocol")]
        public string Protocol { get; set; }

        [PropertyName("http_headers_received")]
        public CrossbarHttpHeadersReceived HttpHeadersReceived { get; set; }

        [PropertyName("http_headers_sent")]
        public CrossbarHttpHeadersSent HttpHeadersSent { get; set; }
    }
}