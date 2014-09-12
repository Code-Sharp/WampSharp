using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampEventSerializer<TMessage>
    {
        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, EventDetails details);

        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments);

        [WampHandler(WampMessageType.v2Event)]
        WampMessage<TMessage> Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords);         
    }
}