using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for a new subscription.
    /// </summary>
    public class WampSubscriptionAddEventArgs : WampSubscriptionEventArgs
    {

        /// <summary>
        /// Initializes a new instance of <see cref="WampSubscriptionAddEventArgs"/>.
        /// </summary>
        /// <param name="sessionId">The session id of the subscribing client.</param>
        /// <param name="observer">A proxy to the subscribing client.</param>
        public WampSubscriptionAddEventArgs(string sessionId, IObserver<object> observer) : 
            base(sessionId)
        {
            Observer = observer;
        }

        /// <summary>
        /// A proxy to the subscribing client.
        /// </summary>
        public IObserver<object> Observer { get; }
    }
}