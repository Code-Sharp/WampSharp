using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface IWampRawTopic<TMessage> : ISubscriptionNotifier
    {
        bool HasSubscribers { get; }
        long SubscriptionId { get; }
        string TopicUri { get; }
        void Subscribe(ISubscribeRequest<TMessage> request, SubscribeOptions options);
        void Unsubscribe(IUnsubscribeRequest<TMessage> request);
    }
}