namespace WampSharp.V2.PubSub
{
    public class RemoteSubscriptionRemoveEventArgs : SubscriptionRemoveEventArgs
    {
        private readonly long mSession;

        public RemoteSubscriptionRemoveEventArgs(long session)
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