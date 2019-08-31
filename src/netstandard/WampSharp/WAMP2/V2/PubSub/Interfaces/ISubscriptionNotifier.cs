using System;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Declares events for <see cref="IWampTopic"/> subscriptions.
    /// </summary>
    public interface ISubscriptionNotifier
    {
        /// <summary>
        /// Occurs before a subscription was added.
        /// </summary>
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;

        /// <summary>
        /// Occurs after a subscription was added.
        /// </summary>
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;

        /// <summary>
        /// Occurs before a subscription was removed.
        /// </summary>
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;

        /// <summary>
        /// Occurs before a subscription was removed.
        /// </summary>
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;

        /// <summary>
        /// Occurs when the topic is empty.
        /// </summary>
        event EventHandler TopicEmpty;
    }
}