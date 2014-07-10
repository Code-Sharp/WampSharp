using System;

namespace WampSharp.V2.Realm
{
    public class WampSessionEventArgs : EventArgs
    {
        private readonly long mSessionId;

        public WampSessionEventArgs(long sessionId)
        {
            mSessionId = sessionId;
        }

        public long SessionId
        {
            get { return mSessionId; }
        }
    }
}