using System;

namespace WampSharp.V2.PubSub
{
    public class WampSubscriptionRemoveEventArgs : EventArgs
    {
        private readonly long mSession;
        private readonly long mSubscriptionId;

        public WampSubscriptionRemoveEventArgs(long session, long subscriptionId)
        {
            mSession = session;
            mSubscriptionId = subscriptionId;
        }

        public long Session
        {
            get
            {
                return mSession;
            }
        }

        public long SubscriptionId
        {
            get
            {
                return mSubscriptionId;
            }
        }
    }
}