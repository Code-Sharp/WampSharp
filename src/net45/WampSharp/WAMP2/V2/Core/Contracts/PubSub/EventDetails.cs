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
    }
}