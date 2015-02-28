using System;

namespace WampSharp.V2.PubSub
{
    public class WampSubscriptionRemoveEventArgs : EventArgs
    {
        private readonly long mSession;

        public WampSubscriptionRemoveEventArgs(long session)
        {
            mSession = session;
        }

        public long Session
        {
            get
            {
                return mSession;
            }
        }
    }
}