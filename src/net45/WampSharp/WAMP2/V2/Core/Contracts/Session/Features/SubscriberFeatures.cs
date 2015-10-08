using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class SubscriberFeatures
    {
        [DataMember(Name = "publisher_identification")]
        public bool? PublisherIdentification { get; internal set; }
    }
}