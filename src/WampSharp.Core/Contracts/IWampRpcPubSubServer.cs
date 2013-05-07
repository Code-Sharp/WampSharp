using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampRpcPubSubServer<TMessage>
    {
        [WampHandler(WampMessageType.Subscribe)]
        void Subscribe(IWampClient client, string topicUri);

        [WampHandler(WampMessageType.Unsubscribe)]
        void Unsubscribe(IWampClient client, string topicUri);

        [WampHandler(WampMessageType.Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event);

        [WampHandler(WampMessageType.Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe);

        [WampHandler(WampMessageType.Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude);

        [WampHandler(WampMessageType.Publish)]
        void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible);         
    }
}