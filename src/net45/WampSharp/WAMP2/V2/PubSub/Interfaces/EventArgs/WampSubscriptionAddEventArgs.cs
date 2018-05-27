using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Occurs when a subscription was added.
    /// </summary>
    public class WampSubscriptionAddEventArgs : EventArgs
    {
        private readonly IRemoteWampTopicSubscriber mSubscriber;

        public WampSubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, SubscribeOptions options)
        {
            Options = options;
            mSubscriber = subscriber;
        }

        /// <summary>
        /// Gets a proxy to the subscribing subscriber.
        /// </summary>
        public IRemoteWampTopicSubscriber Subscriber => mSubscriber;

        /// <summary>
        /// Gets the options the subscriber subscribed with.
        /// </summary>
        public SubscribeOptions Options { get; }
    }
}