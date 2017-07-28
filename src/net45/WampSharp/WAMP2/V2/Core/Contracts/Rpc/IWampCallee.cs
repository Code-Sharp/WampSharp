using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCallee : IWampError<object>, IWampCallee<object>
    {
    }

    public interface IWampCallee<TMessage>
    {
        [WampHandler(WampMessageType.v2Registered)]
        void Registered(long requestId, long registrationId);

        [WampHandler(WampMessageType.v2Unregistered)]
        void Unregistered(long requestId);

        [WampHandler(WampMessageType.v2Invocation)]
        void Invocation(long requestId, long registrationId, InvocationDetails details);

        [WampHandler(WampMessageType.v2Invocation)]
        void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Invocation)]
        void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        [WampHandler(WampMessageType.v2Interrupt)]
        void Interrupt(long requestId, InterruptDetails details);
    }
}