using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class BrokerFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }

        [DataMember(Name = "pattern_based_subscription")]
        public bool? PatternBasedSubscription { get; internal set; }

        [DataMember(Name = "session_meta_api")]
        public bool? SessionMetaApi { get; internal set; }

        [DataMember(Name = "subscription_meta_api")]
        public bool? SubscriptionMetaApi { get; internal set; }

        [DataMember(Name = "subscriber_blackwhite_listing")]
        public bool? SubscriberBlackwhiteListing { get; internal set; }

        [DataMember(Name = "publisher_exclusion")]
        public bool? PublisherExclusion { get; internal set; }

        [DataMember(Name = "subscription_revocation")]
        public bool? SubscriptionRevocation { get; internal set; }

        [DataMember(Name = "event_retention")]
        public bool? EventRetention { get; internal set; }

    }
}