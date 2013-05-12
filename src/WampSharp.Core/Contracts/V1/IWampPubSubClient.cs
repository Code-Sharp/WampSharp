using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampPubSubClient<TMessage>
    {
        [WampHandler(WampMessageType.v1Event)]
        void Event(string topicUri, TMessage @event); 
    }

    public interface IWampPubSubClient : IWampPubSubClient<object>
    {
    }
}