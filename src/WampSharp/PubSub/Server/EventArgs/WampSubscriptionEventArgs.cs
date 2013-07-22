using System;

namespace WampSharp.PubSub.Server
{
    public class WampSubscriptionEventArgs : EventArgs
    {
        private readonly string mSessionId;

        public WampSubscriptionEventArgs(string sessionId)
        {
            mSessionId = sessionId;
        }

        public string SessionId
        {
            get
            {
                return mSessionId;
            }
        }
    }
}