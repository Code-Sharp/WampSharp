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

        private readonly IDictionary<IWampCaller, ICollection<WampRpcInvocation>> mCallerToInvocations =
            new Dictionary<IWampCaller, ICollection<WampRpcInvocation>>();

        private readonly IDictionary<WampCallerRequestKey, WampRpcInvocation> mCallbackToInvocation =
            new Dictionary<WampCallerRequestKey, WampRpcInvocation>();

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

                long invocationId = mRequestIdToInvocation.Add(invocation);

                invocation.InvocationId = invocationId;

                mOperationToInvocations.Add(operation, invocation);

                IWampCaller caller = GetCaller(callback);

                if (caller != null)
                {
                    if (!mCallerToInvocations.ContainsKey(caller))
                    {
                        RegisterDisconnectionNotifier(callback);
                    }

                    mCallerToInvocations.Add(caller, invocation);
                }

                IWampClientProperties properties = invocation.Operation.Callee as IWampClientProperties;

                if (properties.HelloDetails?.Roles?.Callee?.Features?.CallCanceling == true)
                {
                    mCallbackToInvocation.Add(GetRequestKey(caller, callback.RequestId), invocation);
                }

                return invocationId;                
            }
        }

        private static IWampCaller GetCaller(IWampRawRpcOperationRouterCallback callback)
        {
            IWampCaller caller = null;

            if (callback is WampRpcOperationCallback operationCallback)
            {
                caller = operationCallback.Caller;
            }
            return caller;
        }

        public void Cancel(IWampCaller caller, long requestId, CancelOptions options)
        {
            WampCallerRequestKey key = GetRequestKey(caller, requestId);

            lock (mLock)
            {
                if (mCallbackToInvocation.TryGetValue(key, out WampRpcInvocation invocation))
                {
                    IWampCallee callee = invocation.Operation.Callee;

                    callee.Interrupt(invocation.InvocationId, new InterruptDetails() { Mode = options.Mode });
                }
            }
        }

        private static WampCallerRequestKey GetRequestKey(IWampCaller caller, long requestId)
        {
            IWampClientProperties wampClientProperties = caller as IWampClientProperties;
            WampCallerRequestKey callback = new WampCallerRequestKey(wampClientProperties.Session, requestId);
            return callback;
        }

        private void RegisterDisconnectionNotifier(IWampRawRpcOperationRouterCallback callback)
        {

            if (callback is ICallbackDisconnectionNotifier notifier)
            {
                notifier.Disconnected += OnCallbackDisconnected;
            }
        }

        private void OnCallbackDisconnected(object sender, EventArgs e)
        {
            UnregisterDisconnectionNotifier(sender);

            WampRpcOperationCallback callback =
                sender as WampRpcOperationCallback;


            lock (mLock)
            {
                if (mCallerToInvocations.TryGetValue(callback.Caller, out ICollection<WampRpcInvocation> invocations))
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

            if (sender is ICallbackDisconnectionNotifier notifier)
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

                if (mOperationToInvocations.TryGetValue(operation, out ICollection<WampRpcInvocation> invocations))
                {
                    foreach (WampRpcInvocation invocation in invocations.ToArray())
                    {
                        UnregisterInvocation(invocation);

                        invocation.Callback.Error(mFormatter,
                                                  mEmptyDetails, WampErrors.CalleeUnregistered);
                    }
                }
            }
        }

        private WampRpcInvocation GetInvocation(long requestId)
        {
            // This overload only removes - an error is an error

            if (mRequestIdToInvocation.TryGetValue(requestId, out WampRpcInvocation invocation))
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

                mRequestIdToInvocation.TryRemove(invocation.InvocationId, out WampRpcInvocation removedInvocation);

                IWampCaller caller = GetCaller(invocation.Callback);

                if (caller != null)
                {
                    mCallerToInvocations.Remove(caller, invocation);
                }

                mOperationToInvocations.Remove(invocation.Operation, invocation);
                mCallbackToInvocation.Remove(GetRequestKey(caller, invocation.Callback.RequestId));
            }
        }

        private WampRpcInvocation GetInvocation(long requestId, YieldOptions options)
        {
            // This considers the options, since yield can also 
            // return a call progress.

            if (mRequestIdToInvocation.TryGetValue(requestId, out WampRpcInvocation invocation))
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

        private class WampCallerRequestKey
        {
            public WampCallerRequestKey(long session, long requestId)
            {
                Session = session;
                RequestId = requestId;
            }

            public long Session { get; }
            public long RequestId { get; }

            protected bool Equals(WampCallerRequestKey other)
            {
                return Session == other.Session && RequestId == other.RequestId;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((WampCallerRequestKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Session.GetHashCode() * 397) ^ RequestId.GetHashCode();
                }
            }
        }
    }
}