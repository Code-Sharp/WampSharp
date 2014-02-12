using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represnts <see cref="EventArgs"/> for WAMP subscription removal events.
    /// </summary>
    public class WampSubscriptionRemoveEventArgs : WampSubscriptionEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampSubscriptionRemoveEventArgs"/>.
        /// </summary>
        /// <param name="sessionId">The session id of the removed subscriber.</param>
        public WampSubscriptionRemoveEventArgs(string sessionId) : 
            base(sessionId)
        {
        }
    }
}