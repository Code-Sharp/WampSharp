using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class SubscribeRequest<TMessage> : ISubscribeRequest<TMessage>
    {
        private readonly IWampClient<TMessage> mClient;
        private readonly long mRequestId;

        public SubscribeRequest(IWampSubscriber subscriber, long requestId)
        {
            mClient = subscriber as IWampClient<TMessage>;
            mRequestId = requestId;
        }

        public IWampSubscriber Subscriber
        {
            get
            {
                return mClient as IWampSubscriber;
            }
        }

        public IWampClient<TMessage> Client
        {
            get
            {
                return mClient;
            }
        }

        public void Subscribed(long subscriptionId)
        {
            Subscriber.Subscribed(mRequestId, subscriptionId);
        }
    }
}