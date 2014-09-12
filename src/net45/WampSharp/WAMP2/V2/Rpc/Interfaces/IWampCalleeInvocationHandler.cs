using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCalleeInvocationHandler<TMessage> : IWampRpcInvocationCallback<TMessage>
    {
        long RegisterInvocation(IWampRpcOperation operation, IWampRawRpcOperationCallback callback, InvocationDetails options, object[] arguments = null, IDictionary<string, object> argumentsKeywords = null);

        void Error(IWampCallee wampCallee, long requestId, TMessage details, string error);
        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments);

        void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                   TMessage argumentsKeywords);

        void Unregistered(IWampRpcOperation operation);
    }
}