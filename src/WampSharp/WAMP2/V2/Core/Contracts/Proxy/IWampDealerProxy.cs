using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampDealerProxy<TMessage>
    {
        [WampHandler(WampMessageType.v2Register)]
        void Register(long requestId, TMessage options, string procedure);

        [WampHandler(WampMessageType.v2Unregister)]
        void Unregister(long requestId, long registrationId);

        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, TMessage options, string procedure);

        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, TMessage options, string procedure, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);

        [WampHandler(WampMessageType.v2Cancel)]
        void Cancel(long requestId, TMessage options);

        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, TMessage options);

        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, TMessage options, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);
    }
}