using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details for EVENT message.
    /// </summary>
    [DataContract]
    [Serializable]
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
            PublisherAuthenticationId = other.PublisherAuthenticationId;
            PublisherAuthenticationRole = other.PublisherAuthenticationRole;
            Retained = other.Retained;
            OriginalValue = other.OriginalValue;
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

        /// <summary>
        /// Gets the WAMP authrole of the pubisher. Only filled if pubisher is disclosed.
        /// </summary>
        [DataMember(Name = "publisher_authrole")]
        public string PublisherAuthenticationRole { get; internal set; }

        /// <summary>
        /// Gets the WAMP authid of the pubisher. Only filled if pubisher is disclosed.
        /// </summary>
        [DataMember(Name = "publisher_authid")]
        public string PublisherAuthenticationId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the message was retained by the broker on the topic, rather than just published.
        /// </summary>
        [DataMember(Name = "retained")]
        public bool? Retained { get; internal set; }

        [IgnoreDataMember]
        [Obsolete("Use PublisherAuthenticationRole instead.", true)]
        public string AuthenticationRole { get; internal set; }

        [IgnoreDataMember]
        [Obsolete("AuthenticationMethod is no longer sent by the router.", true)]
        public string AuthenticationMethod { get; internal set; }

        [IgnoreDataMember]
        [Obsolete("Use PublisherAuthenticationId instead.", true)]
        public string AuthenticationId { get; internal set; }
    }
}