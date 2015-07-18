using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for a session close event.
    /// </summary>
    public class WampSessionCloseEventArgs
    {
        private readonly string mReason;
        private readonly SessionCloseType mCloseType;
        private readonly long mSessionId;
        private readonly GoodbyeAbortDetails mDetails;

        public WampSessionCloseEventArgs
            (SessionCloseType closeType,
             long sessionId,
             GoodbyeAbortDetails details,
             string reason)
        {
            mReason = reason;
            mCloseType = closeType;
            mSessionId = sessionId;
            mDetails = details;
        }

        /// <summary>
        /// Gets the close reason.
        /// </summary>
        public string Reason
        {
            get
            {
                return mReason;
            }
        }

        /// <summary>
        /// Gets the close type.
        /// </summary>
        public SessionCloseType CloseType
        {
            get
            {
                return mCloseType;
            }
        }

        public long SessionId
        {
            get
            {
                return mSessionId;
            }
        }

        public GoodbyeAbortDetails Details
        {
            get
            {
                return mDetails;
            }
        }
    }
}