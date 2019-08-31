using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for a session close event.
    /// </summary>
    public class WampSessionCloseEventArgs : EventArgs
    {
        private readonly GoodbyeAbortDetails mDetails;

        public WampSessionCloseEventArgs
            (SessionCloseType closeType,
             long sessionId,
             GoodbyeAbortDetails details,
             string reason)
        {
            Reason = reason;
            CloseType = closeType;
            SessionId = sessionId;
            mDetails = details;
        }

        /// <summary>
        /// Gets the close reason.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Gets the close type.
        /// </summary>
        public SessionCloseType CloseType { get; }

        public long SessionId { get; }

        public GoodbyeAbortDetails Details => mDetails;
    }
}