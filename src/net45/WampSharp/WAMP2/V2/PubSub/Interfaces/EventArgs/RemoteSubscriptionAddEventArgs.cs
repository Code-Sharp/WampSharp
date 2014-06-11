using WampSharp.Core.Serialization;

namespace WampSharp.V2.PubSub
{
    public class RemoteSubscriptionAddEventArgs<TMessage> : SubscriptionAddEventArgs<TMessage>
    {
        private readonly long mSession;

        public RemoteSubscriptionAddEventArgs(RemoteWampTopicSubscriber subscriber, TMessage options, IWampFormatter<TMessage> formatter) : 
            base(subscriber, options, formatter)
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