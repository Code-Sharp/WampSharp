using WampSharp.Core.Serialization;

namespace WampSharp.V2.PubSub
{
    public class RemoteSubscriptionAddEventArgs<TMessage> : SubscriptionAddEventArgs
    {
        private readonly long mSession;

        public RemoteSubscriptionAddEventArgs(RemoteWampTopicSubscriber subscriber, TMessage options, IWampFormatter<TMessage> formatter) :
            base(subscriber, new SerializedValue<TMessage>(formatter, options))
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