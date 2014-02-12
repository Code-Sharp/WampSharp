using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampDealer<TMessage>
    {
        [WampHandler(WampMessageType.v2Register)]
        void Register([WampProxyParameter]IWampCallee callee, long requestId, TMessage options, string procedure);

        [WampHandler(WampMessageType.v2Unregister)]
        void Unregister([WampProxyParameter]IWampCallee callee, long requestId, long registrationId);

        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, TMessage options, string procedure);

        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);
    }
}