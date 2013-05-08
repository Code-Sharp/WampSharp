using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampRpcPubSubServer<TMessage>
    {
        [WampHandler(WampMessageType.v1Subscribe)]
        void Subscribe(IWampClient client, string topicUri);

        [WampHandler(WampMessageType.v1Unsubscribe)]
        void Unsubscribe(IWampClient client, string topicUri);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude);

        [WampHandler(WampMessageType.v1Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible);         
    }
}