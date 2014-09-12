using WampSharp.Core.Serialization;

namespace WampSharp.V2.PubSub
{
    public class WampRemoteSubscriptionAddEventArgs<TMessage> : WampSubscriptionAddEventArgs
    {
        private readonly long mSession;

        public WampRemoteSubscriptionAddEventArgs(RemoteWampTopicSubscriber subscriber, TMessage options, IWampFormatter<TMessage> formatter) :
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