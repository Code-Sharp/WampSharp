using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IWampTopicSubscriber
    {
        void Event(long publicationId, EventDetails details);
        void Event(long publicationId, EventDetails details, object[] arguments);
        void Event(long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords);
    }
}