using System;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for a session close event.
    /// </summary>
    public class WampSessionCloseEventArgs : WampSessionEventArgs
    {
        private readonly string mReason;
        private readonly SessionCloseType mCloseType;

        public WampSessionCloseEventArgs
            (SessionCloseType closeType,
             long sessionId,
             ISerializedValue details,
             string reason)
            : base(sessionId, details)
        {
            mReason = reason;
            mCloseType = closeType;
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
    }
}