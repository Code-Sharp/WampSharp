using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class WampSessionDetails
    {
        [DataMember(Name = "realm")]
        public string Realm { get; set; }

        [DataMember(Name = "authprovider")]
        public object AuthProvider { get; set; }

        [DataMember(Name = "authid")]
        public string AuthId { get; set; }

        [DataMember(Name = "authrole")]
        public string AuthRole { get; set; }

        [DataMember(Name = "authmethod")]
        public string AuthMethod { get; set; }

        [DataMember(Name = "session")]
        public long Session { get; set; }

        [DataMember(Name = "transport")]
        public WampTransportDetails TransportDetails { get; set; }
    }
}