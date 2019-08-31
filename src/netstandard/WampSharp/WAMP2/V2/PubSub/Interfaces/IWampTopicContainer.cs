using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents a container for <see cref="IWampTopic"/>s of a given realm.
    /// </summary>
    public interface IWampTopicContainer
    {
        /// <summary>
        /// Subscribes to a topic given its uri.
        /// </summary>
        /// <param name="subscriber">The subscriber to subscribe with.</param>
        /// <param name="topicUri">The topic uri of the topic to subscribe to.</param>
        /// <param name="options">The options to subscribe with.</param>
        /// <returns>A disposable, that will cancel subscription to the topic when disposed.</returns>
        IWampRegistrationSubscriptionToken Subscribe(IWampRawTopicRouterSubscriber subscriber, string topicUri, SubscribeOptions options);

        /// <summary>
        /// Publishes to a requestd topic with requested parameters. 
        /// </summary>
        /// <param name="formatter">The formatter that this publication can be deserialized with.</param>
        /// <param name="options">The publication options.</param>
        /// <param name="topicUri">The topic uri of the topic to publish to.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options, string topicUri);

        /// <summary>
        /// Publishes to a requestd topic with requested parameters. 
        /// </summary>
        /// <param name="formatter">The formatter that this publication can be deserialized with.</param>
        /// <param name="options">The publication options.</param>
        /// <param name="topicUri">The topic uri of the topic to publish to.</param>
        /// <param name="arguments">The published arguments.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options, string topicUri, TMessage[] arguments);

        /// <summary>
        /// Publishes to a requestd topic with requested parameters. 
        /// </summary>
        /// <param name="formatter">The formatter that this publication can be deserialized with.</param>
        /// <param name="options">The publication options.</param>
        /// <param name="topicUri">The topic uri of the topic to publish to.</param>
        /// <param name="arguments">The published arguments.</param>
        /// <param name="argumentKeywords">The published argument keywords.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords);

        /// <summary>
        /// Creates and adds a topic to the container given its uri.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <param name="persistent">A value indicating whether the topic is persistent.</param>
        /// <returns>The created topic.</returns>
        IWampTopic CreateTopicByUri(string topicUri, bool persistent);

        /// <summary>
        /// Gets or creates a topic given its uri.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <returns>The requested topic.</returns>
        IWampTopic GetOrCreateTopicByUri(string topicUri);

        /// <summary>
        /// Gets a topic given it uri.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <returns>The requested topic.</returns>
        IWampTopic GetTopicByUri(string topicUri);

        /// <summary>
        /// Tries to remove a topic given its uri.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <param name="topic">The removed topic.</param>
        /// <returns>A value indicating whether the removal succeeded.</returns>
        bool TryRemoveTopicByUri(string topicUri, out IWampTopic topic);

        /// <summary>
        /// Gets the uris of the topics present in the container.
        /// </summary>
        IEnumerable<string> TopicUris { get; }

        /// <summary>
        /// Gets the topics currently present in the container.
        /// </summary>
        IEnumerable<IWampTopic> Topics { get; }

        /// <summary>
        /// Occurs when a new topic is created.
        /// </summary>
        event EventHandler<WampTopicCreatedEventArgs> TopicCreated;

        /// <summary>
        /// Occurs when a topic is removed.
        /// </summary>
        event EventHandler<WampTopicRemovedEventArgs> TopicRemoved;

        /// <summary>
        /// Creates an id for a topic uri based on options.
        /// </summary>
        /// <param name="topicUri">The topic uri of the subscription.</param>
        /// <param name="options">The subscription options.</param>
        /// <returns>The generated id.</returns>
        /// <remarks>If you don't know what to do here, use a simple 
        /// <see cref="ExactTopicSubscriptionId"/>.</remarks>
        IWampCustomizedSubscriptionId GetSubscriptionId(string topicUri, SubscribeOptions options);

        /// <summary>
        /// Gets all topics (subscriptions) that match the given criteria.
        /// </summary>
        /// <param name="criteria">The given criteria.</param>
        /// <returns>An enumerable consisting of all topics (subscriptions) 
        /// that match the given criteria.</returns>
        IEnumerable<IWampTopic> GetMatchingTopics(string criteria);
    }
}