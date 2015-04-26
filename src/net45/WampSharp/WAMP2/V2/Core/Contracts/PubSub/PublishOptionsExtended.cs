using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class PublishOptionsExtended : PublishOptions
    {
        public PublishOptionsExtended(PublishOptions options) : base(options)
        {
        }

        [IgnoreDataMember]
        public long PublisherId { get; set; }

        [IgnoreDataMember]
        public string TopicUri { get; set; }
    }
}