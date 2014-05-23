using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog<TMessage>
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

        public long Register(IRegisterRequest request, TMessage options, string procedure)
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

        private class WampCalleeRpcOperation : IWampRpcOperation<TMessage>,
            IWampRpcOperation
        {
            private const string CalleeDisconnected = "wamp.error.callee_disconnected";
            private readonly ReaderWriterLockSlim mLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            private readonly ManualResetEvent mResetEvent = new ManualResetEvent(false);
            private readonly IWampCallee mCallee;
            private readonly IWampCalleeInvocationHandler<TMessage> mHandler;
            private readonly WampCalleeOperationCatalog<TMessage> mCatalog;
            private readonly string mProcedure;
            private readonly TMessage mOptions;
            private bool mClientDisconnected = false;

            public WampCalleeRpcOperation(string procedure, IWampCallee callee, TMessage options, IWampCalleeInvocationHandler<TMessage> handler,
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

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther details)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(details);

                this.Invoke(caller, castedOptions);
            }

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther options,
                                       TOther[] arguments)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(options);

                TMessage[] castedArguments =
                    CastArguments(formatter, arguments);

                this.Invoke(caller, castedOptions, castedArguments);
            }

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther options,
                                       TOther[] arguments,
                                       TOther argumentsKeywords)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(options);

                TMessage[] castedArguments =
                    CastArguments(formatter, arguments);

                TMessage castedArgumentsKeywords =
                    formatter.Deserialize<TMessage>(argumentsKeywords);

                this.Invoke(caller, castedOptions, castedArguments, castedArgumentsKeywords);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options)
            {
                InvokePattern(caller, () => InnerInvoke(caller, options));
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
            {
                InvokePattern(caller, () => InnerInvoke(caller, options, arguments));
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
            {
                InvokePattern(caller, () => InnerInvoke(caller, options, arguments, argumentsKeywords));
            }

            private void InnerInvoke(IWampRpcOperationCallback caller, TMessage options)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options);

                Callee.Invocation(requestId, RegistrationId, options);
            }

            private void InnerInvoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments);

                Callee.Invocation(requestId, RegistrationId, options, arguments.Cast<object>().ToArray());
            }

            private void InnerInvoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments,
                                     TMessage argumentsKeywords)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments, argumentsKeywords);

                Callee.Invocation(requestId, RegistrationId, options, arguments.Cast<object>().ToArray(), argumentsKeywords);
            }

            private void InvokePattern(IWampRpcOperationCallback caller, Action action)
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
                        caller.Error(new Dictionary<string, string>(),
                                     CalleeDisconnected);
                    }
                }
                finally
                {
                    mLock.ExitReadLock();
                }
            }

            private static TMessage[] CastArguments<TOther>(IWampFormatter<TOther> formatter, TOther[] arguments)
            {
                return arguments.Select(x =>
                                        formatter.Deserialize<TMessage>(x))
                                .ToArray();
            }

            public void Open()
            {
                // TODO: Race condition: What happens if the callee registers and disconnects
                // TODO: quickly? in this case the invoke will be called after OnClientDisconnect
                // TODO: was called. Problematic.
                mResetEvent.Set();
            }
        }
    }
}