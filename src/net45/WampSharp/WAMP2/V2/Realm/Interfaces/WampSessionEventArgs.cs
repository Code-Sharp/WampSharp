using System;
using WampSharp.V2.Reflection;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Respresents <see cref="EventArgs"/> for a WAMP session event.
    /// </summary>
    public class WampSessionEventArgs : EventArgs
    {
        private readonly long mSessionId;
        private readonly WampTransportDetails mTransportDetails;
        private readonly ISerializedValue mDetails;

        public WampSessionEventArgs(long sessionId, ISerializedValue details)
            : this(sessionId, null, details)
        {
        }

        public WampSessionEventArgs(long sessionId, WampTransportDetails transportDetails, ISerializedValue details)
        {
            mSessionId = sessionId;
            mTransportDetails = transportDetails;
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

        /// <summary>
        /// Gets the transport details associated with this client.
        /// </summary>
        public WampTransportDetails TransportDetails
        {
            get { return mTransportDetails; }
        }
    }
}