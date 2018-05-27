using System;

namespace WampSharp.V1.Core.Listener
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for WAMP session events.
    /// </summary>
    public class WampSessionEventArgs : EventArgs
    {

        /// <summary>
        /// Creates a new instance of <see cref="WampSessionEventArgs"/>.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public WampSessionEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <summary>
        /// The session id.
        /// </summary>
        public string SessionId { get; }
    }
}