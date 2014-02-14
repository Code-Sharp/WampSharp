using System.Collections.Concurrent;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog<TMessage> where TMessage : class
    {
        private readonly IWampIdGenerator mGenerator = new WampIdGenerator();
        private readonly IWampRpcOperationCatalog<TMessage> mCatalog;
        private IWampCalleeInvocationHandler<TMessage> mInvocationHandler;

        private ConcurrentDictionary<long, WampCalleeRpcOperation> mRegistrationIdToOperation =
            new ConcurrentDictionary<long, WampCalleeRpcOperation>();

        public WampCalleeOperationCatalog(IWampRpcOperationCatalog<TMessage> catalog, IWampCalleeInvocationHandler<TMessage> invocationHandler)
        {
            mCatalog = catalog;
            mInvocationHandler = invocationHandler;
        }

        public long Register(IWampCallee callee, TMessage options, string procedure)
        {
            // TODO: Dangerous zone
            long registrationId = mGenerator.Generate();

            WampCalleeRpcOperation operation = 
                new WampCalleeRpcOperation(procedure, callee, registrationId, options, mInvocationHandler, this);
            
            mCatalog.Register(operation);

            mRegistrationIdToOperation[registrationId] = operation;

            return registrationId;
        }

        public void Unregister(IWampCallee callee, long registrationId)
        {
            WampCalleeRpcOperation operation;
            mRegistrationIdToOperation.TryRemove(registrationId, out operation);

            mCatalog.Unregister(operation);
        }

        private class WampCalleeRpcOperation : IWampRpcOperation<TMessage>
        {
            private readonly IWampCallee mCallee;
            private readonly IWampCalleeInvocationHandler<TMessage> mHandler;
            private readonly WampCalleeOperationCatalog<TMessage> mCatalog;
            private readonly string mProcedure;
            private readonly long mRegistrationId;
            private readonly TMessage mOptions;

            public WampCalleeRpcOperation(string procedure, IWampCallee callee, long registrationId, TMessage options, IWampCalleeInvocationHandler<TMessage> handler,
                WampCalleeOperationCatalog<TMessage> catalog)
            {
                mCallee = callee;
                mRegistrationId = registrationId;
                mOptions = options;
                mHandler = handler;
                mCatalog = catalog;
                mProcedure = procedure;

                IWampConnectionMonitor monitor = callee as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnClientDisconnect;
            }

            private void OnClientDisconnect(object sender, System.EventArgs e)
            {
                IWampConnectionMonitor monitor = mCallee as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnClientDisconnect;

                mCatalog.Unregister(mCallee, mRegistrationId);
            }

            public string Procedure
            {
                get
                {
                    return mProcedure;
                }
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);

                mCallee.Invocation(requestId, mRegistrationId, options);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);

                mCallee.Invocation(requestId, mRegistrationId, options, arguments);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);

                mCallee.Invocation(requestId, mRegistrationId, options, arguments, argumentsKeywords);
            }
        }
    }
}