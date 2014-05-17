using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampRpcInvocationCallback<TMessage>
    {
        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, TMessage options);

        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, TMessage options, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);
    }
}