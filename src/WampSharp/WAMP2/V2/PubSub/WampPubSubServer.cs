using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampPubSubServer<TMessage> : IWampPubSubServer<TMessage>
    {
        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topic)
        {
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topic, TMessage[] arguments)
        {
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topic, TMessage[] arguments,
                            TMessage argumentKeywords)
        {
        }

        public void Subscribe(IWampSubscriber subscriber, long requestId, TMessage options, string topicUri)
        {
        }

        public void Unsubscribe(IWampSubscriber subscriber, long requestId, long subscriptionId)
        {
        }
    }
}