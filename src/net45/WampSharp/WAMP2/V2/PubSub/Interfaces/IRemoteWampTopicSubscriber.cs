using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IRemoteWampTopicSubscriber
    {
        void Event(EventDetails details);
        void Event(EventDetails details, object[] arguments);
        void Event(EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords);         
    }
}