using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeInvocationHandler<TMessage> : IWampCalleeInvocationHandler<TMessage>
    {
        private readonly WampIdMapper<WampRpcInvocation<TMessage>> mRequestIdToInvocation =
            new WampIdMapper<WampRpcInvocation<TMessage>>();

        public long RegisterInvocation(IWampRpcOperationCallback caller, TMessage options,
                                       TMessage[] arguments, TMessage argumentsKeywords)
        {
            WampRpcInvocation<TMessage> invocation =
                new WampRpcInvocation<TMessage>
                    (caller, options, arguments, argumentsKeywords);

            long invocationId = mRequestIdToInvocation.Add(invocation);

            invocation.InvocationId = invocationId;

            return invocationId;
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options);
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options, arguments.Cast<object>().ToArray());
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options, arguments.Cast<object>().ToArray(), argumentsKeywords);
        }

        public void Error(IWampCallee wampCallee, long requestId, TMessage details, string error)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error);
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error, arguments.Cast<object>().ToArray());
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error, arguments.Cast<object>().ToArray(), argumentsKeywords);
        }

        private WampRpcInvocation<TMessage> GetInvocation(long requestId)
        {
            // This overload only removes - an error is an error
            WampRpcInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryRemove(requestId, out invocation))
            {
                return invocation;
            }

            return null;
        }

        private WampRpcInvocation<TMessage> GetInvocation(long requestId, TMessage options)
        {
            // This should consider the options, since yield can also 
            // return a call progress.
            WampRpcInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryRemove(requestId, out invocation))
            {
                return invocation;
            }

            return null;
        }
    }
}