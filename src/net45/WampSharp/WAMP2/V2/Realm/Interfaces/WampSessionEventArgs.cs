using System;

namespace WampSharp.V2.Realm
{
    public class WampSessionEventArgs : EventArgs
    {
        private readonly long mSessionId;
        private readonly ISerializedValue mDetails;

        public WampSessionEventArgs(long sessionId, ISerializedValue details)
        {
            mSessionId = sessionId;
            mDetails = details;
        }

        public long SessionId
        {
            get { return mSessionId; }
        }

        public ISerializedValue Details
        {
            get { return mDetails; }
        }
    }
}