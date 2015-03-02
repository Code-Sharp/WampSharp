using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class WampSessionDetails
    {
        [PropertyName("realm")]
        public string Realm { get; set; }

        [PropertyName("authprovider")]
        public object AuthProvider { get; set; }

        [PropertyName("authid")]
        public string AuthId { get; set; }

        [PropertyName("authrole")]
        public string AuthRole { get; set; }

        [PropertyName("authmethod")]
        public string AuthMethod { get; set; }

        [PropertyName("session")]
        public long Session { get; set; }

        [PropertyName("transport")]
        public TransportDetails TransportDetails { get; set; }
    }
}