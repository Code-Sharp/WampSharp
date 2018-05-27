using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using WampSharp.V2.Core;
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
                IWampRegistrationSubscriptionToken token = 
                    mCatalog.Register(operation, options);

                long registrationId = token.TokenId;

                operation.RegistrationId = registrationId;

                CompositeDisposable disposable = new CompositeDisposable(token, operation);

                bool alreadyRegistered = 
                    !mOperationToDisposable.TryAdd(operation, disposable);

                request.Registered(registrationId);

                // If the operation is already registered, ignore it.
                if (!alreadyRegistered)
                {
                    operation.Open();
                }

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


            if (!mOperationToDisposable.TryRemove(operation, out IDisposable disposable))
            {
                throw new WampException(WampErrors.NoSuchRegistration, "registrationId: " + registrationId);
            }

            disposable.Dispose();

            mInvocationHandler.Unregistered(operation);
        }
    }
}