using System.Runtime.Serialization;
using WampSharp.Core.Message;
using WampSharp.V2.Reflection;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Hello)]
    public class HelloDetails : WampDetailsOptions
    {
        /// <summary>
        /// Gets the announced WAMP roles.
        /// </summary>
        [DataMember(Name = "roles")]
        public ClientRoles Roles { get; internal set; }

        /// <summary>
        /// Gets the announced authentication methods.
        /// </summary>
        [DataMember(Name = "authmethods")]
        public string[] AuthenticationMethods { get; internal set; }

        /// <summary>
        /// Gets the announced authentication ID.
        /// </summary>
        [DataMember(Name = "authid")]
        public string AuthenticationId { get; internal set; }

        /// <summary>
        /// Gets the transport details associated with this client.
        /// </summary>
        [IgnoreDataMember]
        public WampTransportDetails TransportDetails { get; internal set; }
    }

    [DataContract]
    public class RouterRoles
    {
        [DataMember(Name = "dealer")]
        public Role<DealerFeatures> Dealer { get; internal set; }

        [DataMember(Name = "broker")]
        public Role<BrokerFeatures> Broker { get; internal set; }
    }

    [DataContract]
    public class DealerFeatures
    {
        [DataMember(Name = "pattern_based_registration")]
        public bool? PatternBasedRegistration { get; internal set; }

        [DataMember(Name = "registration_revocation")]
        public bool? RegistrationRevocation { get; internal set; }

        [DataMember(Name = "shared_registration")]
        public bool? SharedRegistration { get; internal set; }
        
        [DataMember(Name = "caller_identification")]
        public bool? CallerIdentification { get; internal set; }
        
        [DataMember(Name = "registration_meta_api")]
        public bool? RegistrationMetaApi { get; internal set; }

        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }
    }

    [DataContract]
    public class BrokerFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }

        [DataMember(Name = "pattern_based_subscription")]
        public bool? PatternBasedSubscription { get; internal set; }

        [DataMember(Name = "subscription_meta_api")]
        public bool? SubscriptionMetaApi { get; internal set; }

        [DataMember(Name = "subscription_revocation")]
        public bool? SubscriptionRevocation { get; internal set; }

        [DataMember(Name = "publisher_exclusion")]
        public bool? PublisherExclusion { get; internal set; }

        [DataMember(Name = "subscriber_blackwhite_listing")]
        public bool? SubscriberBlackwhiteListing { get; internal set; }
    }

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

    [DataContract]
    public class Role<TFeatures>
    {
        [DataMember(Name = "features")]
        public TFeatures Features { get; set; }

        public static implicit operator Role<TFeatures>(TFeatures features)
        {
            return new Role<TFeatures>() {Features = features};
        }
    }

    [DataContract]
    public class CallerFeatures
    {
        [DataMember(Name = "caller_identification")]
        public bool? CallerIdentification { get; internal set; }

        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }
    }

    [DataContract]
    public class CalleeFeatures
    {
        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }
    }

    [DataContract]
    public class PublisherFeatures
    {
        [DataMember(Name = "subscriber_blackwhite_listing")]
        public bool? SubscriberBlackwhiteListing { get; internal set; }

        [DataMember(Name = "publisher_exclusion")]
        public bool? PublisherExclusion { get; internal set; }

        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }
    }

    [DataContract]
    public class SubscriberFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }
    }
}