using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface IWampRawTopicContainer<TMessage>
    {
        long Subscribe(ISubscribeRequest<TMessage> request, SubscribeOptions options, string topicUri);
        void Unsubscribe(IUnsubscribeRequest<TMessage> request, long subscriptionId);
        long Publish(PublishOptions options, string topicUri);
        long Publish(PublishOptions options, string topicUri, TMessage[] arguments);
        long Publish(PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords);
    }
}