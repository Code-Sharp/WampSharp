using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampRemoteSubscriptionAddEventArgs<TMessage> : WampSubscriptionAddEventArgs
    {
        private readonly long mSession;

        public WampRemoteSubscriptionAddEventArgs(RemoteWampTopicSubscriber subscriber, SubscribeOptions options) :
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