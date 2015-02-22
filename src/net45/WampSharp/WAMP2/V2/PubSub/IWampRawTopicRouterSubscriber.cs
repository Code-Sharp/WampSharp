using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents a topic subscriber that lives in the same process as the router.
    /// </summary>
    public interface IWampRawTopicRouterSubscriber
    {
        /// <summary>
        /// Occurs when an event arrives.
        /// </summary>
        /// <param name="formatter">The formatted this event can be deserialized with.</param>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="options">The publication options.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options);

        /// <summary>
        /// Occurs when an event arrives.
        /// </summary>
        /// <param name="formatter">The formatted this event can be deserialized with.</param>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="options">The publication options.</param>
        /// <param name="arguments">The arguments of this event.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments);

        /// <summary>
        /// Occurs when an event arrives.
        /// </summary>
        /// <param name="formatter">The formatted this event can be deserialized with.</param>
        /// <param name="publicationId">The publication id of this event.</param>
        /// <param name="options">The publication options.</param>
        /// <param name="arguments">The arguments of this event.</param>
        /// <param name="argumentsKeywords">The argument keywords of this event.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}