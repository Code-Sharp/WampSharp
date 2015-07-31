using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampCalleeInvocationHandler<TMessage> : IWampCalleeInvocationHandler<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        private readonly WampIdMapper<WampRpcInvocation> mRequestIdToInvocation =
            new WampIdMapper<WampRpcInvocation>();

        private readonly IDictionary<RemoteWampCalleeDetails, ICollection<WampRpcInvocation>> mOperationToInvocations =
            new Dictionary<RemoteWampCalleeDetails, ICollection<WampRpcInvocation>>();

        private readonly IDictionary<IWampRawRpcOperationRouterCallback, ICollection<WampRpcInvocation>> mCallbackToInvocations =
            new Dictionary<IWampRawRpcOperationRouterCallback, ICollection<WampRpcInvocation>>();

        private readonly object mLock = new object();
        private readonly TMessage mEmptyDetails;

        public WampCalleeInvocationHandler(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mEmptyDetails = mFormatter.Serialize(new Dictionary<string, string>());
        }

        public long RegisterInvocation(RemoteWampCalleeDetails operation, IWampRawRpcOperationRouterCallback callback, InvocationDetails options, object[] arguments = null, IDictionary<string, object> argumentsKeywords = null)
        {
            lock (mLock)
            {
                WampRpcInvocation invocation =
                    new WampRpcInvocation
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

        private void RegisterDisconnectionNotifier(IWampRawRpcOperationRouterCallback callback)
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

            IWampRawRpcOperationRouterCallback callback =
                sender as IWampRawRpcOperationRouterCallback;

            ICollection<WampRpcInvocation> invocations;

            lock (mLock)
            {
                if (mCallbackToInvocations.TryGetValue(callback, out invocations))
                {
                    foreach (WampRpcInvocation invocation in invocations.ToArray())
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

        public void Yield(IWampCallee callee, long requestId, YieldOptions options)
        {
            ResultArrived(callee, requestId, options,
                          invocation =>
                              invocation.Callback.Result(mFormatter, options));
        }

        public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments)
        {
            ResultArrived(callee, requestId, options,
                          invocation =>
                              invocation.Callback.Result(mFormatter, options, arguments));
        }

        public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            ResultArrived(callee, requestId, options,
                          invocation =>
                              invocation.Callback.Result(mFormatter, options, arguments, argumentsKeywords));
        }

        private void ResultArrived(IWampCallee callee, long requestId, YieldOptions options, Action<WampRpcInvocation> action)
        {
            WampRpcInvocation invocation = GetInvocation(requestId, options);

            if (invocation != null)
            {
                if (invocation.Operation.Callee == callee)
                {
                    action(invocation);
                }
                else
                {
                    callee.InvocationError(requestId, mEmptyDetails, WampErrors.NotAuthorized);
                }
            }
        }

        public void Error(IWampCallee wampCallee, long requestId, TMessage details, string error)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(mFormatter, details, error));
        }

        public void Error(IWampClientProxy wampCallee, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(mFormatter, details, error, arguments));
        }

        public void Error(IWampClientProxy wampCallee, long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            ErrorArrived(requestId,
                         invocation => invocation.Callback.Error(mFormatter, details, error, arguments, argumentsKeywords));
        }

        private void ErrorArrived(long requestId, Action<WampRpcInvocation> action)
        {
            WampRpcInvocation invocation = GetInvocation(requestId);

            if (invocation != null)
            {
                action(invocation);
            }
        }

        public void Unregistered(RemoteWampCalleeDetails operation)
        {
            lock (mLock)
            {
                ICollection<WampRpcInvocation> invocations;

                if (mOperationToInvocations.TryGetValue(operation, out invocations))
                {
                    foreach (WampRpcInvocation invocation in invocations.ToArray())
                    {
                        UnregisterInvocation(invocation);

                        invocation.Callback.Error(mFormatter,
                                                  mEmptyDetails,
                                                  "wamp.error.callee_unregistered");
                    }
                }
            }
        }

        private WampRpcInvocation GetInvocation(long requestId)
        {
            // This overload only removes - an error is an error
            WampRpcInvocation invocation;

            if (mRequestIdToInvocation.TryGetValue(requestId, out invocation))
            {
                UnregisterInvocation(invocation);
                return invocation;
            }

            return null;
        }

        private void UnregisterInvocation(WampRpcInvocation invocation)
        {
            lock (mLock)
            {
                WampRpcInvocation removedInvocation;

                mRequestIdToInvocation.TryRemove(invocation.InvocationId, out removedInvocation);
                mCallbackToInvocations.Remove(invocation.Callback, invocation);
                mOperationToInvocations.Remove(invocation.Operation, invocation);
            }
        }

        private WampRpcInvocation GetInvocation(long requestId, YieldOptions options)
        {
            // This considers the options, since yield can also 
            // return a call progress.
            WampRpcInvocation invocation;

            if (mRequestIdToInvocation.TryGetValue(requestId, out invocation))
            {
                bool progressiveResult = options.Progress == true;
                
                if (!progressiveResult)
                {
                    UnregisterInvocation(invocation);
                    return invocation;
                }

                bool requestedProgress = invocation.Options.ReceiveProgress == true;

                if (progressiveResult && requestedProgress)
                {
                    return invocation;
                }
            }

            return null;
        }
    }
}