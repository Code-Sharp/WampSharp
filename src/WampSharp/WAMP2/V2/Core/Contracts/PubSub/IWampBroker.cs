using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampBroker<TMessage>
    {
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampPublisher publisher, long requestId, TMessage options, string topicUri);
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampPublisher publisher, long requestId, TMessage options, string topicUri, TMessage[] arguments);
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampPublisher publisher, long requestId, TMessage options, string topicUri, TMessage[] arguments, TMessage argumentKeywords);
        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe([WampProxyParameter]IWampSubscriber subscriber, long requestId, TMessage options, string topicUri);
        [WampHandler(WampMessageType.v2Subscribe)]
        void Unsubscribe([WampProxyParameter]IWampSubscriber subscriber, long requestId, long subscriptionId);
    }
}