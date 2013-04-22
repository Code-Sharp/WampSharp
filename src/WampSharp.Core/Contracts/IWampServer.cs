using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampServer : IWampServer<object>
    {
    }

    public interface IWampServer<TMessage>
    {
        [WampHandler(WampMessageType.Prefix)]
        void Prefix(IWampClient client, string prefix, string uri);

        [WampHandler(WampMessageType.Call)]
        void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments);

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