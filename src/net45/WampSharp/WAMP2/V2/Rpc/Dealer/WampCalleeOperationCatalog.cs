using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog
    {
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mInvocationHandler;
        private readonly ConcurrentDictionary<RemoteWampCalleeDetails, IDisposable> mOperationToDisposable =
            new ConcurrentDictionary<RemoteWampCalleeDetails, IDisposable>(); 

        public WampCalleeOperationCatalog(IWampRpcOperationCatalog catalog, IWampCalleeInvocationHandler<TMessage> invocationHandler)
        {
            mCatalog = catalog;
            mInvocationHandler = invocationHandler;
        }

        public long Register(IRegisterRequest request, RegisterOptions options, string procedure)
        {
            WampCalleeRpcOperation<TMessage> operation =
                new WampCalleeRpcOperation<TMessage>(procedure,
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
                
                mOperationToDisposable[operation] = 
                    new CompositeDisposable(token, operation);
                
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
            RemoteWampCalleeDetails operation = 
                new RemoteWampCalleeDetails(callee, registrationId);

            IDisposable disposable;

            if (!mOperationToDisposable.TryRemove(operation, out disposable))
            {
                throw new WampException(WampErrors.NoSuchRegistration, "registrationId: " + registrationId);
            }

            disposable.Dispose();

            mInvocationHandler.Unregistered(operation);
        }
    }
}