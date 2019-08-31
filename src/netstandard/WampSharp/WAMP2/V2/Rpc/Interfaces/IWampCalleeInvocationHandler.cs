using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal interface IWampCalleeInvocationHandler<TMessage> : IWampRpcInvocationCallback<TMessage>
    {
        long RegisterInvocation(RemoteWampCalleeDetails operation, IWampRawRpcOperationRouterCallback callback, InvocationDetails options, object[] arguments = null, IDictionary<string, object> argumentsKeywords = null);

        void Cancel(IWampCaller caller, long requestId, CancelOptions options);

        void Error(IWampCallee wampCallee, long requestId, TMessage details, string error);
        void Error(IWampClientProxy wampCallee, long requestId, TMessage details, string error, TMessage[] arguments);

        void Error(IWampClientProxy wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                   TMessage argumentsKeywords);

        void Unregistered(RemoteWampCalleeDetails operation);
    }
}