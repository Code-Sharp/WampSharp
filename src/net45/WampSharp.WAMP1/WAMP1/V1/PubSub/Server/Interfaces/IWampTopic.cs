using System;
using System.Reactive.Subjects;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents a WAMP topic, that some clients are subscribed to.
    /// </summary>
    public interface IWampTopic :
        ISubject<object>, 
        ISubject<WampNotification, object>,
        IDisposable
    {
        /// <summary>
        /// Gets the uri of the topic.
        /// </summary>
        string TopicUri { get; }

        /// <summary>
        /// Gets a value indicating whether this topic is persistent.
        /// </summary>
        bool Persistent { get; }

        /// <summary>
        /// Gets a value indicating whether this topic has observers.
        /// </summary>
        bool HasObservers { get; }

        /// <summary>
        /// Occurs when a new subscription is being added to the topic.
        /// </summary>
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;

        /// <summary>
        /// Occurs after a new subscription has been added to the topic.
        /// </summary>
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;

        /// <summary>
        /// Occurs when a subscription is being removed from the topic.
        /// </summary>
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;

        /// <summary>
        /// Occurs after a subscription has been removed from the topic.
        /// </summary>
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;

        /// <summary>
        /// Occurs when the topic is empty.
        /// </summary>
        event EventHandler TopicEmpty; 

        /// <summary>
        /// Unsubscribes a subscriber given its session id from the topic.
        /// </summary>
        /// <param name="sessionId">The subscriber's session id.</param>
        void Unsubscribe(string sessionId);
    }
}