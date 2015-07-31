using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class UnsubscribeRequest<TMessage> : IUnsubscribeRequest<TMessage>
    {
        private readonly IWampClientProxy<TMessage> mClient;
        private readonly long mRequestId;
        private readonly long mSubscriptionId;

        public UnsubscribeRequest(IWampSubscriber subscriber, long requestId, long subscriptionId)
        {
            mClient = subscriber as IWampClientProxy<TMessage>;
            mRequestId = requestId;
            mSubscriptionId = subscriptionId;
        }

        public IWampClientProxy<TMessage> Client
        {
            get
            {
                return mClient;
            }
        }

        public void Unsubscribed()
        {
            Client.Unsubscribed(mRequestId, mSubscriptionId);
        }
    }
}