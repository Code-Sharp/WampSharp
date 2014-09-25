using System;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Respresents <see cref="EventArgs"/> for a WAMP session event.
    /// </summary>
    public class WampSessionEventArgs : EventArgs
    {
        private readonly long mSessionId;
        private readonly ISerializedValue mDetails;

        public WampSessionEventArgs(long sessionId, ISerializedValue details)
        {
            mSessionId = sessionId;
            mDetails = details;
        }
        
        /// <summary>
        /// Gets the relevant session id.
        /// </summary>
        public long SessionId
        {
            get { return mSessionId; }
        }

        /// <summary>
        /// Gets the details associated with this event.
        /// </summary>
        public ISerializedValue Details
        {
            get { return mDetails; }
        }
    }
}