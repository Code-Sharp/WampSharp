using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class SubscribeRequest<TMessage> : ISubscribeRequest<TMessage>
    {
        private readonly IWampClientProxy<TMessage> mClient;
        private readonly long mRequestId;

        public SubscribeRequest(IWampSubscriber subscriber, long requestId)
        {
            mClient = subscriber as IWampClientProxy<TMessage>;
            mRequestId = requestId;
        }

        public IWampSubscriber Subscriber
        {
            get
            {
                return mClient as IWampSubscriber;
            }
        }

        public IWampClientProxy<TMessage> Client
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