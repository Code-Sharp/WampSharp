using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents a proxy to a remote topic subscriber.
    /// </summary>
    public interface IRemoteWampTopicSubscriber
    {
        /// <summary>
        /// Gets the subscription id of the subscriber.
        /// </summary>
        long SubscriptionId { get; }

        /// <summary>
        /// Gets the session id of the subscriber.
        /// </summary>
        long SessionId { get; }

        /// <summary>
        /// Gets the authentication id of the subscriber.
        /// </summary>
        string AuthenticationId { get; }

        /// <summary>
        /// Gets the authentication role of the subscriber.
        /// </summary>
        string AuthenticationRole { get; }

        /// <summary>
        /// Publishes an EVENT message with the given parameters.
        /// </summary>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="details">The details to publish.</param>
        void Event(long publicationId, EventDetails details);

        /// <summary>
        /// Publishes an EVENT message with the given parameters.
        /// </summary>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="details">The details to publish.</param>
        /// <param name="arguments">The arguments to publish.</param>
        void Event(long publicationId, EventDetails details, object[] arguments);

        /// <summary>
        /// Publishes an EVENT message with the given parameters.
        /// </summary>
        /// <param name="details">The details to publish.</param>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="arguments">The arguments to publish.</param>
        /// <param name="argumentsKeywords">The arguments keywords to publish.</param>
        void Event(long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords);         
    }
}