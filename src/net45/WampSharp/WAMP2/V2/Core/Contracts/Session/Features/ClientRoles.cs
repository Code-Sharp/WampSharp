using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class ClientRoles
    {
        [DataMember(Name = "caller")]
        public Role<CallerFeatures> Caller { get; internal set; }

        [DataMember(Name = "callee")]
        public Role<CalleeFeatures> Callee { get; internal set; }

        [DataMember(Name = "publisher")]
        public Role<PublisherFeatures> Publisher { get; internal set; }

        [DataMember(Name = "subscriber")]
        public Role<SubscriberFeatures> Subscriber { get; internal set; }
    }
}