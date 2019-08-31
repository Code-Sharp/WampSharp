using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    public class PublishOptionsExtended : PublishOptions
    {
        public PublishOptionsExtended(PublishOptions options) : base(options)
        {
        }

        [IgnoreDataMember]
        public long PublisherId { get; set; }

        [IgnoreDataMember]
        public string TopicUri { get; set; }

        [IgnoreDataMember]
        public string AuthenticationRole { get; internal set; }

        [IgnoreDataMember]
        public string AuthenticationId { get; internal set; }
    }
}