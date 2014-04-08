using System;

namespace WampSharp.Core.Listener.V1
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for WAMP session events.
    /// </summary>
    public class WampSessionEventArgs : EventArgs
    {
        private readonly string mSessionId;

        /// <summary>
        /// Creates a new instance of <see cref="WampSessionEventArgs"/>.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public WampSessionEventArgs(string sessionId)
        {
            mSessionId = sessionId;
        }

        /// <summary>
        /// The session id.
        /// </summary>
        public string SessionId
        {
            get
            {
                return mSessionId;
            }
        }
    }
}