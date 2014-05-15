using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeInvocationHandler<TMessage> : IWampCalleeInvocationHandler<TMessage>
    {
        private readonly WampIdMapper<WampRpcInvocation<TMessage>> mRequestIdToInvocation =
            new WampIdMapper<WampRpcInvocation<TMessage>>();

        private IDictionary<IWampRpcOperation, ICollection<WampRpcInvocation<TMessage>>> mOperationToInvocations = 
            new Dictionary<IWampRpcOperation, ICollection<WampRpcInvocation<TMessage>>>();

        private IDictionary<IWampRpcOperationCallback, ICollection<WampRpcInvocation<TMessage>>> mCallbackToInvocations =
            new Dictionary<IWampRpcOperationCallback, ICollection<WampRpcInvocation<TMessage>>>();

        private readonly object mLock = new object();

        public long RegisterInvocation(IWampRpcOperation operation, IWampRpcOperationCallback callback, TMessage options, TMessage[] arguments = null, TMessage argumentsKeywords = default(TMessage))
        {
            lock (mLock)
            {
                WampRpcInvocation<TMessage> invocation =
                    new WampRpcInvocation<TMessage>
                        (operation, callback, options, arguments, argumentsKeywords);

                if (!mCallbackToInvocations.ContainsKey(callback))
                {
                    RegisterDisconnectionNotifier(callback);
                }

                long invocationId = mRequestIdToInvocation.Add(invocation);

                invocation.InvocationId = invocationId;

                mOperationToInvocations.Add(operation, invocation);
                mCallbackToInvocations.Add(callback, invocation);

                return invocationId;                
            }
        }

        private void RegisterDisconnectionNotifier(IWampRpcOperationCallback callback)
        {
            ICallbackDisconnectionNotifier notifier = callback as ICallbackDisconnectionNotifier;

            if (notifier != null)
            {
                notifier.Disconnected += OnCallbackDisconnected;
            }
        }

        private void OnCallbackDisconnected(object sender, EventArgs e)
        {
            UnregisterDisconnectionNotifier(sender);

            IWampRpcOperationCallback callback = 
                sender as IWampRpcOperationCallback;

            ICollection<WampRpcInvocation<TMessage>> invocations;

            lock (mLock)
            {
                if (mCallbackToInvocations.TryGetValue(callback, out invocations))
                {
                    foreach (WampRpcInvocation<TMessage> invocation in invocations.ToArray())
                    {
                        UnregisterInvocation(invocation);
                    }
                }
            }
        }

        private void UnregisterDisconnectionNotifier(object sender)
        {
            ICallbackDisconnectionNotifier notifier = sender as ICallbackDisconnectionNotifier;
            
            if (notifier != null)
            {
                notifier.Disconnected -= OnCallbackDisconnected;
            }
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options)
        {
            ResultArrived(requestId, options,
                          invocation =>
                          invocation.Callback.Result(options));
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments)
        {
            ResultArrived(requestId, options,
                          invocation =>
                          invocation.Callback.Result(options, arguments.Cast<object>().ToArray()));
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            ResultArrived(requestId, options,
                          invocation =>
                          invocation.Callback.Result(options, arguments.Cast<object>().ToArray(), argumentsKeywords));
        }

        private void ResultArrived(long requestId, TMessage options, Action<WampRpcInvocation<TMessage>> action)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId, options);

            if (invocation != null)
            {
                action(invocation);
            }
        }

        public void Error(IWampCallee wampCallee, long requestId, TMessage details, string error)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(details, error));
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(details, error, arguments.Cast<object>().ToArray()));
        }

        public void Error(IWampClient wampCallee, long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(details, error, arguments.Cast<object>().ToArray(), argumentsKeywords));
        }

        private void ErrorArrived(long requestId, Action<WampRpcInvocation<TMessage>> action)
        {
            WampRpcInvocation<TMessage> invocation = GetInvocation(requestId);

            if (invocation != null)
            {
                action(invocation);
            }
        }

        public void Unregistered(IWampRpcOperation operation)
        {
            ICollection<WampRpcInvocation<TMessage>> invocations;
            
            lock (mLock)
            {
                if (mOperationToInvocations.TryGetValue(operation, out invocations))
                {
                    foreach (WampRpcInvocation<TMessage> invocation in invocations.ToArray())
                    {
                        UnregisterInvocation(invocation);

                        invocation.Callback.Error(new Dictionary<string, object>(),
                                                  "wamp.error.callee_unregistered");
                    }
                }
            }
        }

        private WampRpcInvocation<TMessage> GetInvocation(long requestId)
        {
            // This overload only removes - an error is an error
            WampRpcInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryGetValue(requestId, out invocation))
            {
                UnregisterInvocation(invocation);
                return invocation;
            }

            return null;
        }

        private void UnregisterInvocation(WampRpcInvocation<TMessage> invocation)
        {
            lock (mLock)
            {
                WampRpcInvocation<TMessage> removedInvocation;

                mRequestIdToInvocation.TryRemove(invocation.InvocationId, out removedInvocation);
                mCallbackToInvocations.Remove(invocation.Callback, invocation);
                mOperationToInvocations.Remove(invocation.Operation, invocation);
            }
        }

        private WampRpcInvocation<TMessage> GetInvocation(long requestId, TMessage options)
        {
            // This should consider the options, since yield can also 
            // return a call progress.
            WampRpcInvocation<TMessage> invocation;

            if (mRequestIdToInvocation.TryGetValue(requestId, out invocation))
            {
                UnregisterInvocation(invocation);
                return invocation;
            }

            return null;
        }
    }
}