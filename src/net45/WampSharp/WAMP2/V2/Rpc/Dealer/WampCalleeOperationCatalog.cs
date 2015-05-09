using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog
    {
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mInvocationHandler;
        private readonly ConcurrentDictionary<IWampRpcOperation, IDisposable> mOperationToDisposable =
            new ConcurrentDictionary<IWampRpcOperation, IDisposable>(); 

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

            try
            {
                IWampRpcOperationRegistrationToken token = 
                    mCatalog.Register(operation, options);

                long registrationId = token.RegistrationId;

                operation.RegistrationId = registrationId;
                
                mOperationToDisposable[operation] = token;
                
                request.Registered(registrationId);

                operation.Open();

                return registrationId;
            }
            catch (Exception)
            {
                operation.Dispose();
                throw;
            }
        }

        public void Unregister(IWampCallee callee, long registrationId)
        {
            WampCalleeRpcOperation operation = 
                new WampCalleeRpcOperation(callee, registrationId);

            IDisposable disposable;

            if (!mOperationToDisposable.TryRemove(operation, out disposable))
            {
                throw new WampException(WampErrors.NoSuchRegistration, "registrationId: " + registrationId);
            }

            disposable.Dispose();
            operation.Dispose();

            mInvocationHandler.Unregistered(operation);
        }

        private class WampCalleeRpcOperation : IWampRpcOperation, IDisposable
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

            public WampCalleeRpcOperation(IWampCallee callee, long registrationId)
            {
                mCallee = callee;
                RegistrationId = registrationId;
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

            public RegisterOptions Options
            {
                get { return mOptions; }
            }

            public void Invoke<TOther>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details)
            {
                this.Invoke(caller, details);
            }

            public void Invoke<TOther>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details, TOther[] arguments)
            {
                this.Invoke(caller, details, arguments.Cast<object>().ToArray());
            }

            public void Invoke<TOther>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TOther> formatter, InvocationDetails details, TOther[] arguments, IDictionary<string, TOther> argumentsKeywords)
            {
                this.Invoke(caller, details, arguments.Cast<object>().ToArray(), argumentsKeywords.ToDictionary(x => x.Key, x => (object)x.Value));
            }

            public void Invoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails details)
            {
                InvokePattern(caller, details, invocationDetails => InnerInvoke(caller, invocationDetails));
            }

            public void Invoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails details, object[] arguments)
            {
                InvokePattern(caller, details, invocationDetails => InnerInvoke(caller, invocationDetails, arguments));
            }

            public void Invoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                InvokePattern(caller, details, invocationDetails => InnerInvoke(caller, invocationDetails, arguments, argumentsKeywords));
            }

            private void InnerInvoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails options)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options);

                Callee.Invocation(requestId, RegistrationId, options);
            }

            private void InnerInvoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails options, object[] arguments)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments);

                Callee.Invocation(requestId, RegistrationId, options, arguments);
            }

            private void InnerInvoke(IWampRawRpcOperationRouterCallback caller, InvocationDetails options, object[] arguments,
                                     IDictionary<string, object> argumentsKeywords)
            {
                long requestId =
                    mHandler.RegisterInvocation(this, caller, options, arguments, argumentsKeywords);

                Callee.Invocation(requestId, RegistrationId, options, arguments, argumentsKeywords);
            }

            private void InvokePattern(IWampRawRpcOperationRouterCallback caller, InvocationDetails details, Action<InvocationDetails> action)
            {
                mResetEvent.WaitOne();

                try
                {
                    mLock.EnterReadLock();

                    if (!mClientDisconnected)
                    {
                        var detailsForCallee = GetInvocationDetails(details);
                        action(detailsForCallee);
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

            private InvocationDetails GetInvocationDetails(InvocationDetails details)
            {
                InvocationDetailsExtended casted = details as InvocationDetailsExtended;

                if (casted == null)
                {
                    return details;
                }

                InvocationDetails result = new InvocationDetails(casted);

                CallOptions callerOptions = casted.CallerOptions;

                if (Options.DiscloseCaller == true ||
                    callerOptions.DiscloseMe == true)
                {
                    result.Caller = casted.CallerSession;
                }

                if (callerOptions.ReceiveProgress == true)
                {
                    result.ReceiveProgress = true;
                }

                return result;
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

            protected bool Equals(WampCalleeRpcOperation other)
            {
                return Equals(mCallee, other.mCallee) &&
                       (string.Equals(mProcedure, other.mProcedure) ||
                        RegistrationId == other.RegistrationId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((WampCalleeRpcOperation)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (mCallee != null ? mCallee.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }
    }
}