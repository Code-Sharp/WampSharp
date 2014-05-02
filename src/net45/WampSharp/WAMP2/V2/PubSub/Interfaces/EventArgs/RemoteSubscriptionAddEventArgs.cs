namespace WampSharp.V2.PubSub
{
    public class RemoteSubscriptionAddEventArgs : SubscriptionAddEventArgs
    {
        private readonly long mSession;

        public RemoteSubscriptionAddEventArgs(RemoteWampTopicSubscriber subscriber, object options) : 
            base(subscriber, options)
        {
            mSession = subscriber.Session;
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