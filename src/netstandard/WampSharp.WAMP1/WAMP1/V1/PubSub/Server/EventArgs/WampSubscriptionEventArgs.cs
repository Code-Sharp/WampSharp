using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for WAMP subscription events.
    /// </summary>
    public class WampSubscriptionEventArgs : System.EventArgs
    {

        /// <summary>
        /// Creates a new instance of <see cref="WampSubscriptionEventArgs"/>.
        /// </summary>
        /// <param name="sessionId">The session id of the current subscriber.</param>
        public WampSubscriptionEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <summary>
        /// The session id of the subscriber.
        /// </summary>
        public string SessionId { get; }
    }
}