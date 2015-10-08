using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class SubscriptionDetails
    {
        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "id")]
        public long SubscriptionId { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }

        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
    }
}