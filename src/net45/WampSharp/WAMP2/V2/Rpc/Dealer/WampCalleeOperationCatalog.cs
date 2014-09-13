using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog
    {
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mInvocationHandler;

        private readonly WampIdMapper<WampCalleeRpcOperation> mRegistrationIdToOperation =
            new WampIdMapper<WampCalleeRpcOperation>();

        public WampCalleeOperationCatalog(IWampRpcOperationCatalog catalog, IWampCalleeInvocationHandler<TMessage> invocationHandler)
        {
            mCatalog = catalog;
            mInvocationHandler = invocationHandler;
        }

        public long Register(IRegisterRequest request, RegisterOptions options, string procedure)
        {
            WampCalleeRpcOperation operation =
                new WampCalleeRpcOperation(procedure,
                                           request.Callee,
                                           options,
                                           mInvocationHandler,
                                           this);

            long registrationId = 
                mRegistrationIdToOperation.Add(operation);

            operation.RegistrationId = registrationId; // Hate this setter.

            try
            {
                mCatalog.Register(operation);

                request.Registered(registrationId);

                operation.Open();

                return registrationId;
            }
            catch (Exception ex)
            {
                operation.Dispose();

                WampCalleeRpcOperation removedOperation;

                mRegistrationIdToOperation.TryRemove
                    (registrationId, out removedOperation);

                throw;
            }
        }

        public void Unregister(IWampCallee callee, long registrationId)
        {
            WampCalleeRpcOperation operation;
            
            if (!mRegistrationIdToOperation.TryGetValue(registrationId, out operation))
            {
                throw new WampException(WampErrors.NoSuchRegistration, "registrationId: " + registrationId);
            }

            if (operation.Callee != callee)
            {
                throw new WampException(WampErrors.NotAuthorized, "registrationId: " + registrationId);
            }

            mRegistrationIdToOperation.TryRemove(registrationId, out operation);
            mCatalog.Unregister(operation);
            mInvocationHandler.Unregistered(operation);
        }

        private class WampCalleeRpcOperation : IWampRpcOperation<object>,
            IWampRpcOperation, IDisposable
        {
            private const string CalleeDisconnected = "wamp.error.callee_disconnected";
            private readonly ReaderWriterLockSlim mLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            private readonly ManualResetEvent mResetEvent = new ManualResetEvent(false);
            private readonly IWampCallee mCallee;
            private readonly IWampCalleeInvocationHandler<TMessage> mHandler;
            private readonly WampCalleeOperationCatalog<TMessage> mCatalog;
            private readonly string mProcedure;
            private readonly RegisterOptions mOptions;
            private bool mClientDisconnected = false;

            public WampCalleeRpcOperation(string procedure, IWampCallee callee, RegisterOptions options, IWampCalleeInvocationHandler<TMessage> handler,
                WampCalleeOperationCatalog<TMessage> catalog)
            {
                mCallee = callee;
                mOptions = options;
                mHandler = handler;
                mCatalog = catalog;
                mProcedure = procedure;

                IWampConnectionMonitor monitor = callee as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnClientDisconnect;
            }

            private void OnClientDisconnect(object sender, EventArgs e)
            {
                try
                {
                    mLock.EnterWriteLock();

                    mClientDisconnected = true;

                    IWampConnectionMonitor monitor = Callee as IWampConnectionMonitor;
                    monitor.ConnectionClosed -= OnClientDisconnect;

                    mCatalog.Unregister(Callee, RegistrationId);
                    mHandler.Unregistered(this);
                }
                finally
                {
                    mLock.ExitWriteLock();
                }
            }

            public string Procedure
            {
                get
                {
                    return mProcedure;
                }
            }

            public long RegistrationId
            {
                get; 
                set;
            }

            public IWampCallee Callee
            {
                get
                {
                    return mCallee;
                }
            }

            public void Invoke<TOther>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details)
            {
                this.Invoke(caller, details);
            }

            public void Invoke<TOther>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details, TOther[] arguments)
            {
                this.Invoke(caller, details, arguments.Cast<object>().ToArray());
            }

            public void Invoke<TOther>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details, TOther[] arguments, IDictionary<string, TOther> argumentsKeywords)
            {
                this.Invoke(caller, details, arguments.Cast<object>().ToArray(), argumentsKeywords.ToDictionary(x => x.Key, x => (object)x.Value));
            }

            public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details)
            {
                InvokePattern(caller, () => InnerInvoke(caller, details));
            }

            public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, object[] arguments)
            {
                InvokePattern(caller, () => InnerInvoke(caller, details, arguments));
            }

            public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                InvokePattern(caller, () => InnerInvoke(caller, details, arguments, argumentsKeywords));
            }

            private void InnerInvoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails options)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options);

                Callee.Invocation(requestId, RegistrationId, options);
            }

            private void InnerInvoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails options, object[] arguments)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments);

                Callee.Invocation(requestId, RegistrationId, options, arguments);
            }

            private void InnerInvoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails options, object[] arguments,
                                     IDictionary<string, object> argumentsKeywords)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments, argumentsKeywords);

                Callee.Invocation(requestId, RegistrationId, options, arguments, argumentsKeywords);
            }

            private void InvokePattern(IWampRouterRawRpcOperationCallback caller, Action action)
            {
                mResetEvent.WaitOne();

                try
                {
                    mLock.EnterReadLock();

                    if (!mClientDisconnected)
                    {
                        action();
                    }
                    else
                    {
                        caller.Error(WampObjectFormatter.Value,
                                     new Dictionary<string, string>(),
                                     CalleeDisconnected);
                    }
                }
                finally
                {
                    mLock.ExitReadLock();
                }
            }

            public void Open()
            {
                mResetEvent.Set();
            }

            public void Dispose()
            {
                IWampConnectionMonitor monitor = mCallee as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnClientDisconnect;
            }
        }
    }
}