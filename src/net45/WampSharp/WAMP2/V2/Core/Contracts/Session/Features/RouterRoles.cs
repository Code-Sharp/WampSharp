using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class RouterRoles
    {
        [DataMember(Name = "dealer")]
        public Role<DealerFeatures> Dealer { get; internal set; }

        [DataMember(Name = "broker")]
        public Role<BrokerFeatures> Broker { get; internal set; }
    }
}