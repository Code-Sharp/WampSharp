using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IWampRawTopicContainer<TMessage>
    {
        long Subscribe(IWampSubscriber subscriber, TMessage options, string topicUri);
        void Unsubscribe(IWampSubscriber subscriber, long subscriptionId);
        long Publish(TMessage options, string topicUri);
        long Publish(TMessage options, string topicUri, TMessage[] arguments);
        long Publish(TMessage options, string topicUri, TMessage[] arguments, TMessage argumentKeywords);
    }
}