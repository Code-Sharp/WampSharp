using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class PublisherFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }

        [DataMember(Name = "subscriber_blackwhite_listing")]
        public bool? SubscriberBlackwhiteListing { get; internal set; }

        [DataMember(Name = "publisher_exclusion")]
        public bool? PublisherExclusion { get; internal set; }
    }
}