using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCalleeInvocationHandler<TMessage> : IWampRpcInvocationCallback<TMessage>
    {
        long RegisterInvocation(IWampRpcOperation operation, IWampRpcOperationCallback callback, TMessage options, TMessage[] arguments = null, TMessage argumentsKeywords = default(TMessage));

        void Error(IWampCallee wampCallee, long requestId, TMessage details, string error);
        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments);

        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                   TMessage argumentsKeywords);

        void Unregistered(IWampRpcOperation operation);
    }
}