using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampBroker<TMessage>
    {
        [WampHandler(WampMessageType.v2CallError)]
        void Publish([WampProxyParameter]IWampSubscriber publisher, string topic);

        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampSubscriber publisher, string topic, TMessage @event);

        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampSubscriber publisher, string topic, TMessage @event, TMessage publishOptions);

        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe([WampProxyParameter]IWampSubscriber subscriber, string topic);

        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe([WampProxyParameter]IWampSubscriber subscriber, string topic, TMessage subscribeOptions);

        [WampHandler(WampMessageType.v2Unsubscribe)]
        void Unsubscribe([WampProxyParameter]IWampSubscriber subscriber, string topic);

        [WampHandler(WampMessageType.v2Unsubscribe)]
        void Unsubscribe([WampProxyParameter]IWampSubscriber subscriber, string topic, TMessage subscribeOptions);
    }
}