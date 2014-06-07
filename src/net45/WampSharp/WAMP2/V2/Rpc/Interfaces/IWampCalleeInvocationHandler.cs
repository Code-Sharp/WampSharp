using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCalleeInvocationHandler<TMessage> : IWampRpcInvocationCallback<TMessage>
    {
        long RegisterInvocation(IWampRpcOperation operation, IWampRawRpcOperationCallback callback, object options, object[] arguments = null, object argumentsKeywords = null);

        void Error(IWampCallee wampCallee, long requestId, TMessage details, string error);
        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments);

        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                   TMessage argumentsKeywords);

        void Unregistered(IWampRpcOperation operation);
    }
}