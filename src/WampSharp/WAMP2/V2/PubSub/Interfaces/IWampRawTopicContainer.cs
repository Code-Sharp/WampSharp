using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface IWampRawTopicContainer<TMessage>
    {
        long Subscribe(ISubscribeRequest<TMessage> request, TMessage options, string topicUri);
        void Unsubscribe(IUnsubscribeRequest<TMessage> request, long subscriptionId);
        long Publish(TMessage options, string topicUri);
        long Publish(TMessage options, string topicUri, TMessage[] arguments);
        long Publish(TMessage options, string topicUri, TMessage[] arguments, TMessage argumentKeywords);
    }
}