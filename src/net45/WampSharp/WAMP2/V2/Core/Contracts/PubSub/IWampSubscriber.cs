using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSubscriber : IWampSubscriber<object>, IWampError<object>
    {
    }

    public interface IWampSubscriber<TMessage>
    {
        [WampHandler(WampMessageType.v2Subscribed)]
        void Subscribed(long requestId, long subscriptionId);

        [WampHandler(WampMessageType.v2Unsubscribed)]
        void Unsubscribed(long requestId);

        [WampHandler(WampMessageType.v2Event)]
        void Event(long subscriptionId, long publicationId, EventDetails details);

        [WampHandler(WampMessageType.v2Event)]
        void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Event)]
        void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}