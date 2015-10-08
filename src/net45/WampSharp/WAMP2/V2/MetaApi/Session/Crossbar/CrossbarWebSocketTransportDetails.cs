using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class CrossbarWebSocketTransportDetails : WampTransportDetails
    {
        [DataMember(Name = "peer")]
        public string Peer { get; set; }

        [DataMember(Name = "protocol")]
        public string Protocol { get; set; }

        [DataMember(Name = "http_headers_received")]
        public CrossbarHttpHeadersReceived HttpHeadersReceived { get; set; }

        [DataMember(Name = "http_headers_sent")]
        public CrossbarHttpHeadersSent HttpHeadersSent { get; set; }
    }
}