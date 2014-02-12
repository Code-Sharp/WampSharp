using System;
using System.Collections.Generic;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents a container for <see cref="IWampTopic"/>s.
    /// </summary>
    public interface IWampTopicContainer
    {
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
        /// <param name="persistent">A value indicating whether the topic is persistent.</param>
        /// <returns>The requested topic.</returns>
        IWampTopic GetOrCreateTopicByUri(string topicUri, bool persistent);
        
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
    }
}