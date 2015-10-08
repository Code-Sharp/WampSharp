using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampCallee<TMessage> : 
        IWampRpcOperationRegistrationProxy, IWampCallee<TMessage>,
        IWampCalleeError<TMessage>
    {
        private readonly IWampServerProxy mProxy;

        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampClientConnectionMonitor mMonitor;

        private readonly WampRequestIdMapper<RegisterRequest> mPendingRegistrations =
            new WampRequestIdMapper<RegisterRequest>();

        private readonly ConcurrentDictionary<long, IWampRpcOperation> mRegistrations =
            new ConcurrentDictionary<long, IWampRpcOperation>();

        private readonly WampRequestIdMapper<UnregisterRequest> mPendingUnregistrations =
            new WampRequestIdMapper<UnregisterRequest>();

        public WampCallee(IWampServerProxy proxy, IWampFormatter<TMessage> formatter, IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            mFormatter = formatter;
            mMonitor = monitor;

            monitor.ConnectionBroken += OnConnectionBroken;
            monitor.ConnectionError += OnConnectionError;
        }

        public Task<IAsyncDisposable> Register(IWampRpcOperation operation, RegisterOptions options)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            RegisterRequest registerRequest =
                new RegisterRequest(operation, mFormatter);

            long id = mPendingRegistrations.Add(registerRequest);

            registerRequest.RequestId = id;

            mProxy.Register(id, options, operation.Procedure);

            return registerRequest.Task;
        }

        public void Registered(long requestId, long registrationId)
        {
            RegisterRequest registerRequest;

            if (mPendingRegistrations.TryRemove(requestId, out registerRequest))
            {
                mRegistrations[registrationId] = registerRequest.Operation;
                registerRequest.Complete(new UnregisterDisposable(this, registrationId));
            }
        }

        private bool IsConnected
        {
            get { return mMonitor.IsConnected; }
        }

        private class UnregisterDisposable : IAsyncDisposable
        {
            private readonly WampCallee<TMessage> mCallee;
            private readonly long mRegistrationId;

            public UnregisterDisposable(WampCallee<TMessage> callee, long registrationId)
            {
                mCallee = callee;
                mRegistrationId = registrationId;
            }

            public Task DisposeAsync()
            {
                if (!mCallee.IsConnected)
                {
                    throw new WampSessionNotEstablishedException();
                }

                return mCallee.Unregister(mRegistrationId);
            }
        }

        private Task Unregister(long registrationId)
        {
            UnregisterRequest unregisterRequest =
                new UnregisterRequest(mFormatter);

            long requestId = mPendingUnregistrations.Add(unregisterRequest);

            unregisterRequest.RequestId = requestId;

            mProxy.Unregister(requestId, registrationId);

            return unregisterRequest.Task;
        }

        public void Unregistered(long requestId)
        {
            UnregisterRequest unregisterRequest;

            if (mPendingUnregistrations.TryRemove(requestId, out unregisterRequest))
            {
                IWampRpcOperation operation;
                mRegistrations.TryRemove(requestId, out operation);
                unregisterRequest.Complete();
            }
        }

        private IWampRpcOperation TryGetOperation(long registrationId)
        {
            IWampRpcOperation operation;

            if (mRegistrations.TryGetValue(registrationId, out operation))
            {
                return operation;
            }

            return null;
        }

        private IWampRawRpcOperationRouterCallback GetCallback(long requestId)
        {
            return new ServerProxyCallback(mProxy, requestId);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details)
        {
            InvocationPattern(requestId, registrationId, details,
                              (operation, callback, invocationDetails) =>
                                  operation.Invoke(callback,
                                                   mFormatter,
                                                   invocationDetails));
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments)
        {
            InvocationPattern(requestId, registrationId, details,
                              (operation, callback, invocationDetails) =>
                                  operation.Invoke(callback,
                                                   mFormatter,
                                                   invocationDetails,
                                                   arguments));
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            InvocationPattern(requestId, registrationId, details,
                              (operation, callback, invocationDetails) =>
                                  operation.Invoke(callback,
                                                   mFormatter,
                                                   invocationDetails,
                                                   arguments,
                                                   argumentsKeywords));
        }

        private void InvocationPattern(long requestId, long registrationId, InvocationDetails details, Action<IWampRpcOperation, IWampRawRpcOperationRouterCallback, InvocationDetails> invocationAction)
        {
            IWampRpcOperation operation = TryGetOperation(registrationId);

            if (operation != null)
            {
                IWampRawRpcOperationRouterCallback callback = GetCallback(requestId);

                InvocationDetails modifiedDetails = new InvocationDetails(details)
                {
                    Procedure = details.Procedure ?? operation.Procedure
                };

                invocationAction(operation, callback, modifiedDetails);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error)
        {
            RegisterRequest registerRequest;

            if (mPendingRegistrations.TryRemove(requestId, out registerRequest))
            {
                registerRequest.Error(details, error);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            RegisterRequest registerRequest;

            if (mPendingRegistrations.TryRemove(requestId, out registerRequest))
            {
                registerRequest.Error(details, error, arguments);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            RegisterRequest registerRequest;

            if (mPendingRegistrations.TryRemove(requestId, out registerRequest))
            {
                registerRequest.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error)
        {
            UnregisterRequest unregisterRequest;

            if (mPendingUnregistrations.TryRemove(requestId, out unregisterRequest))
            {
                unregisterRequest.Error(details, error);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            UnregisterRequest unregisterRequest;

            if (mPendingUnregistrations.TryRemove(requestId, out unregisterRequest))
            {
                unregisterRequest.Error(details, error, arguments);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            UnregisterRequest unregisterRequest;

            if (mPendingUnregistrations.TryRemove(requestId, out unregisterRequest))
            {
                unregisterRequest.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void Interrupt(long requestId, TMessage options)
        {
            throw new NotImplementedException();
        }

        private class RegisterRequest : WampPendingRequest<TMessage, IAsyncDisposable>
        {
            private readonly IWampRpcOperation mOperation;

            public RegisterRequest(IWampRpcOperation operation, IWampFormatter<TMessage> formatter) :
                base(formatter)
            {
                mOperation = operation;
            }

            public IWampRpcOperation Operation
            {
                get
                {
                    return mOperation;
                }
            }
        }

        private class UnregisterRequest : WampPendingRequest<TMessage>
        {
            public UnregisterRequest(IWampFormatter<TMessage> formatter) : base(formatter)
            {
            }
        }

        public void OnConnectionError(object sender, WampConnectionErrorEventArgs eventArgs)
        {
            Exception exception = eventArgs.Exception;

            mPendingRegistrations.ConnectionError(exception);
            mPendingUnregistrations.ConnectionError(exception);

            Cleanup();
        }

        public void OnConnectionBroken(object sender, WampSessionCloseEventArgs eventArgs)
        {
            mPendingRegistrations.ConnectionClosed(eventArgs);
            mPendingUnregistrations.ConnectionClosed(eventArgs);
            Cleanup();
        }

        private void Cleanup()
        {
            // TODO: clean up other things?
            mRegistrations.Clear();
        }

        private class ServerProxyCallback : IWampRawRpcOperationRouterCallback
        {
            private readonly IWampServerProxy mProxy;
            private readonly long mRequestId;

            public ServerProxyCallback(IWampServerProxy proxy, long requestId)
            {
                mProxy = proxy;
                mRequestId = requestId;
            }

            public long RequestId
            {
                get
                {
                    return mRequestId;
                }
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, YieldOptions options)
            {
                mProxy.Yield(RequestId, options);
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, YieldOptions options, TResult[] arguments)
            {
                mProxy.Yield(RequestId, options, arguments.Cast<object>().ToArray());
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, YieldOptions options, TResult[] arguments, IDictionary<string, TResult> argumentsKeywords)
            {
                mProxy.Yield(RequestId, options, arguments.Cast<object>().ToArray(), argumentsKeywords.ToDictionary(x => x.Key, x => (object)x.Value));
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error)
            {
                mProxy.InvocationError(RequestId, details, error);
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments)
            {
                mProxy.InvocationError(RequestId, details, error, arguments.Cast<object>().ToArray());
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments, TResult argumentsKeywords)
            {
                mProxy.InvocationError(RequestId, details, error, arguments.Cast<object>().ToArray(), argumentsKeywords);
            }
        }
    }
}