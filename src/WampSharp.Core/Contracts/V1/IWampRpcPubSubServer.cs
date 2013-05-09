using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampRpcPubSubServer<TMessage>
    {
        [WampHandler(WampMessageType.v1Subscribe)]
        void Subscribe([WampProxyParameter]IWampClient client, string topicUri);

        [WampHandler(WampMessageType.v1Unsubscribe)]
        void Unsubscribe([WampProxyParameter]IWampClient client, string topicUri);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, bool excludeMe);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, string[] exclude);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible);         
    }
}