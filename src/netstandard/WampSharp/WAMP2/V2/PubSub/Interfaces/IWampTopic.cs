using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents a WAMP topic.
    /// </summary>
    public interface IWampTopic : ISubscriptionNotifier, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this topic has subscribers.
        /// </summary>
        bool HasSubscribers { get; }

        /// <summary>
        /// Gets the topic uri of this topic.
        /// </summary>
        string TopicUri { get; }

        /// <summary>
        /// Publishes a message to the topic
        /// </summary>
        /// <param name="formatter">The formatter been used to publish the message.</param>
        /// <param name="publicationId"></param>
        /// <param name="publishOptions">The options of the publication.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns>The publication id.</returns>
        void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions);

        /// <summary>
        /// Publishes a message to the topic
        /// </summary>
        /// <param name="formatter">The formatter been used to publish the message.</param>
        /// <param name="publicationId"></param>
        /// <param name="publishOptions">The options of the publication.</param>
        /// <param name="arguments">The arguments to publish.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns>The publication id.</returns>
        void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions, TMessage[] arguments);

        /// <summary>
        /// Publishes a message to the topic
        /// </summary>
        /// <param name="formatter">The formatter been used to publish the message.</param>
        /// <param name="publicationId"></param>
        /// <param name="publishOptions">The options of the publication.</param>
        /// <param name="arguments">The arguments to publish.</param>
        /// <param name="argumentKeywords">The arguments keywords to publish.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns>The publication id.</returns>
        void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords);

        /// <summary>
        /// Subscribes a given subscriber to the topic.
        /// </summary>
        /// <param name="subscriber">The given subscriber.</param>
        /// <returns>A disposable, when disposed the subscription will be canceled.</returns>
        IDisposable Subscribe(IWampRawTopicRouterSubscriber subscriber);

        /// <summary>
        /// Gets the subscription id associated with this subscription.
        /// </summary>
        long SubscriptionId { get; }
    }
}