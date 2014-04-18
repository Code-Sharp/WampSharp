using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampBrokerProxy<TMessage>
    {
        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, TMessage options, string topicUri);

        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, TMessage options, string topicUri, TMessage[] arguments);
        
        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, TMessage options, string topicUri, TMessage[] arguments, TMessage argumentKeywords);
        
        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe(long requestId, TMessage options, string topicUri);
        
        [WampHandler(WampMessageType.v2Unsubscribe)]
        void Unsubscribe(long requestId, long subscriptionId);
    }
}