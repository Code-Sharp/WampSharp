using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class SubscribeRequest<TMessage> : ISubscribeRequest<TMessage>
    {
        private readonly long mRequestId;

        public SubscribeRequest(IWampSubscriber subscriber, long requestId)
        {
            Client = subscriber as IWampClientProxy<TMessage>;
            mRequestId = requestId;
        }

        public IWampSubscriber Subscriber => Client as IWampSubscriber;

        public IWampClientProxy<TMessage> Client { get; }

        public void Subscribed(long subscriptionId)
        {
            Subscriber.Subscribed(mRequestId, subscriptionId);
        }
    }
}