using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details for EVENT message.
    /// </summary>
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Event)]
    public class EventDetails : WampDetailsOptions
    {
        public EventDetails()
        {
        }

        public EventDetails(EventDetails other)
        {
            Publisher = other.Publisher;
            Topic = other.Topic;
            AuthenticationId = other.AuthenticationId;
            AuthenticationMethod = other.AuthenticationMethod;
            AuthenticationRole = other.AuthenticationRole;
        }

        /// <summary>
        /// Gets or sets the publisher id of this publication.
        /// </summary>
        [DataMember(Name = "publisher")]
        public long? Publisher { get; internal set; }

        /// <summary>
        /// Gets or sets the topic of this publication.
        /// </summary>
        [DataMember(Name = "topic")]
        public string Topic { get; internal set; }

        [ExperimentalWampFeature]
        [DataMember(Name = "authrole")]
        public string AuthenticationRole { get; internal set; }

        [ExperimentalWampFeature]
        [DataMember(Name = "authmethod")]
        public string AuthenticationMethod { get; internal set; }

        [ExperimentalWampFeature]
        [DataMember(Name = "authid")]
        public string AuthenticationId { get; internal set; }
    }
}