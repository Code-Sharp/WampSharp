using System.Collections.Concurrent;
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
                new WampCalleeRpcOperation(procedure, callee, registrationId, options, mInvocationHandler);
            
            mCatalog.Register(operation);

            mRegistrationIdToOperation[registrationId] = operation;

            return registrationId;
        }

        public void Unregister(IWampCallee callee)
        {
            // TODO: Implement here what happens when a client disconnects.
        }

        public void Unregister(IWampCallee callee, long registrationId)
        {
            WampCalleeRpcOperation operation;
            mRegistrationIdToOperation.TryGetValue(registrationId, out operation);

            mCatalog.Unregister(operation);
        }

        private class WampCalleeRpcOperation : IWampRpcOperation<TMessage>
        {
            private readonly IWampCallee mCallee;
            private readonly IWampCalleeInvocationHandler<TMessage> mHandler; 
            private readonly string mProcedure;
            private readonly long mRegistrationId;
            private readonly TMessage mOptions;

            public WampCalleeRpcOperation(string procedure, IWampCallee callee, long registrationId, TMessage options, IWampCalleeInvocationHandler<TMessage> handler)
            {
                mCallee = callee;
                mRegistrationId = registrationId;
                mOptions = options;
                mHandler = handler;
                mProcedure = procedure;
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
                
                mCallee.Invocation(mRegistrationId, requestId, options);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);
                
                mCallee.Invocation(mRegistrationId, requestId, options, arguments);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);
                
                mCallee.Invocation(mRegistrationId, requestId, options, arguments, argumentsKeywords);
            }
        }
    }
}