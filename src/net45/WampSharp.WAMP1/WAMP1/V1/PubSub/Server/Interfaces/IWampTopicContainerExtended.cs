using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// A <see cref="IWampTopicContainer"/> with extra thread-safe
    /// functionality.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampTopicContainerExtended<TMessage> : IWampTopicContainer
    {
        /// <summary>
        /// Subscribes to a topic given its uri.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <param name="observer">The observer to subscribe to the topic with.</param>
        /// <returns>A disposable used to unsubscribe from the topic.</returns>
        IDisposable Subscribe(string topicUri, IObserver<object> observer);

        /// <summary>
        /// Unsubscribes an observer from the topic given its session id.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <param name="sessionId">The observer's session id.</param>
        void Unsubscribe(string topicUri, string sessionId);
        
        /// <summary>
        /// Publishes an event to a given topic.
        /// </summary>
        /// <param name="topicUri">The topic's uri.</param>
        /// <param name="event">The event to publish.</param>
        /// <param name="exclude">An array of excluded subscribers' session ids.</param>
        /// <param name="eligible">An array of eligible subscribers' session ids.</param>
        void Publish(string topicUri, TMessage @event, string[] exclude, string[] eligible);         
    }
}