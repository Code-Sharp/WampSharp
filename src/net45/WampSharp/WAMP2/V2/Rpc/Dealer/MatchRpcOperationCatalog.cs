using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal abstract class MatchRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, WampProcedureRegistration> mProcedureToRegistration =
            new ConcurrentDictionary<string, WampProcedureRegistration>();

        private readonly WampIdMapper<WampProcedureRegistration> mRegistrationIdToRegistration;

        private readonly object mLock = new object();

        protected MatchRpcOperationCatalog(WampIdMapper<WampProcedureRegistration> mapper)
        {
            mRegistrationIdToRegistration = mapper;
        }

        public IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            lock (mLock)
            {
                WampProcedureRegistration registration =
                    mProcedureToRegistration
                        .GetOrAdd
                        (operation.Procedure,
                         procedureUri => CreateRegistration(registerOptions, procedureUri));

                return registration.Register(operation, registerOptions);                
            }
        }

        private WampProcedureRegistration CreateRegistration(RegisterOptions registerOptions, string procedureUri)
        {
            WampProcedureRegistration result = new WampProcedureRegistration(procedureUri, registerOptions);

            result.Empty += OnRegistrationEmpty;

            long registrationId = mRegistrationIdToRegistration.Add(result);

            result.RegistrationId = registrationId;

            OnRegistrationAdded(result);

            return result;
        }

        // These hooks will be changed in meta-api version
        protected virtual void OnRegistrationAdded(WampProcedureRegistration procedureUri)
        {
            RaiseRegistrationAdded(procedureUri);
        }

        protected virtual void OnRegistrationRemoved(WampProcedureRegistration procedureUri)
        {
            RaiseRegistrationRemoved(procedureUri);
        }

        private void OnRegistrationEmpty(object sender, EventArgs e)
        {
            WampProcedureRegistration registration = sender as WampProcedureRegistration;

            if (!registration.HasOperations)
            {
                lock (mLock)
                {
                    if (!registration.HasOperations)
                    {
                        mRegistrationIdToRegistration.TryRemoveExact(registration.RegistrationId, registration);
                        mProcedureToRegistration.TryRemoveExact(registration.Procedure, registration);
                        registration.Empty -= OnRegistrationEmpty;
                        OnRegistrationRemoved(registration);
                    }
                }
            }
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            return InvokePattern(procedure,
                                 operation =>
                                     operation.Invoke(caller, formatter, details));
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            return InvokePattern(procedure,
                                 operation =>
                                     operation.Invoke(caller, formatter, details, arguments));
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            return InvokePattern(procedure,
                                 operation =>
                                     operation.Invoke(caller, formatter, details, arguments, argumentsKeywords));
        }

        private IWampCancellableInvocation InvokePattern(string procedure, Func<IWampRpcOperation, IWampCancellableInvocation> invokeAction)
        {
            IWampRpcOperation operation = TryGetOperation(procedure);

            if (operation == null)
            {
                return null;
            }
            else
            {
                return invokeAction(operation);
            }
        }

        private IWampRpcOperation TryGetOperation(string procedure)
        {
            return GetMatchingOperation(procedure);
        }

        protected IWampRpcOperation GetOperationByUri(string procedureUri)
        {

            if (mProcedureToRegistration.TryGetValue(procedureUri, out WampProcedureRegistration result))
            {
                return result;
            }

            return null;
        }

        protected IEnumerable<IWampRpcOperation> Operations => this.mProcedureToRegistration.Values;

        public event EventHandler<WampProcedureRegisterEventArgs> RegistrationAdded;

        public event EventHandler<WampProcedureRegisterEventArgs> RegistrationRemoved;

        private void RaiseRegistrationAdded(IWampProcedureRegistration registration)
        {
            RegistrationAdded?.Invoke(this, new WampProcedureRegisterEventArgs(registration));
        }

        private void RaiseRegistrationRemoved(IWampProcedureRegistration registration)
        {
            RegistrationRemoved?.Invoke(this, new WampProcedureRegisterEventArgs(registration));
        }

        #region Abstract methods

        public abstract bool Handles(RegisterOptions options);

        public abstract IWampRpcOperation GetMatchingOperation(string criteria);

        #endregion
    }
}