using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IRawWampTopic<TMessage>
    {
        long SubscriptionId { get; }
        string TopicUri { get; }
        void Subscribe(IWampSubscriber subscriber, TMessage options);
        void Unsubscribe(IWampSubscriber subscriber);
    }
}