using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class SubscriberFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }

        [DataMember(Name = "pattern_based_subscription")]
        public bool? PatternBasedSubscription { get; internal set; }

        [DataMember(Name = "subscription_revocation")]
        public bool? SubscriptionRevocation { get; internal set; }
    }
}