using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class WebSocketTransportDetails : TransportDetails
    {
        [PropertyName("peer")]
        public string Peer { get; set; }

        [PropertyName("protocol")]
        public string Protocol { get; set; }

        [PropertyName("http_headers_received")]
        public HttpHeadersReceived HttpHeadersReceived { get; set; }

        [PropertyName("http_headers_sent")]
        public HttpHeadersSent HttpHeadersSent { get; set; }
    }
}