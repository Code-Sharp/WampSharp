using System.Collections.Concurrent;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeInvocationHandler<TMessage> : IWampCalleeInvocationHandler<TMessage> where TMessage : class
    {
        private IWampIdGenerator mGenerator = new WampIdGenerator();

        private ConcurrentDictionary<long, WampCalleeInvocation<TMessage>> mRequestIdToInvocation =
            new ConcurrentDictionary<long, WampCalleeInvocation<TMessage>>();

        public long RegisterInvocation(IWampRpcOperationCallback caller, TMessage options,
                                       TMessage[] arguments, TMessage argumentsKeywords)
        {
            WampCalleeInvocation<TMessage> invocation =
                new WampCalleeInvocation<TMessage>
                    (caller, options, arguments, argumentsKeywords);

            long invocationId = mGenerator.Generate();

            // TODO: yuck
            while (!mRequestIdToInvocation.TryAdd(invocationId, invocation))
            {
                invocationId = mGenerator.Generate();
            }

            return invocationId;
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options);
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options, arguments);
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId, options);

            invocation.Caller.Result(options, arguments, argumentsKeywords);
        }

        public void Error(IWampCallee wampCallee, long requestId, TMessage details, string error)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error);
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error, arguments);
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            WampCalleeInvocation<TMessage> invocation = GetInvocation(requestId);

            invocation.Caller.Error(details, error, arguments, argumentsKeywords);
        }

        private WampCalleeInvocation<TMessage> GetInvocation(long requestId)
        {
            // This overload only removes - an error is an error
            WampCalleeInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryRemove(requestId, out invocation))
            {
                return invocation;
            }

            return null;
        }

        private WampCalleeInvocation<TMessage> GetInvocation(long requestId, TMessage options)
        {
            // This should consider the options, since yield can also 
            // return a call progress.
            WampCalleeInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryRemove(requestId, out invocation))
            {
                return invocation;
            }

            return null;
        }
    }
}