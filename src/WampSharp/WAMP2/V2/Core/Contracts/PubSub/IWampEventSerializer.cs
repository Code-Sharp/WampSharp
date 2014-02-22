using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampEventSerializer<TMessage>
    {
        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, object details);

        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, object details, object[] arguments);

        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, object details, object[] arguments, object argumentsKeywords);         
    }
}