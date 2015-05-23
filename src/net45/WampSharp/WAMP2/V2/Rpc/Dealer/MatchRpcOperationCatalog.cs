using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal abstract class MatchRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, ProcedureRegistration> mProcedureToRegistration =
            new ConcurrentDictionary<string, ProcedureRegistration>();

        private readonly WampIdMapper<ProcedureRegistration> mRegistrationIdToRegistration;

        private readonly object mLock = new object();

        protected MatchRpcOperationCatalog(WampIdMapper<ProcedureRegistration> mapper)
        {
            mRegistrationIdToRegistration = mapper;
        }

        public IWampRpcOperationRegistrationToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            lock (mLock)
            {
                ProcedureRegistration registration =
                    mProcedureToRegistration
                        .GetOrAdd
                        (operation.Procedure,
                         procedureUri => CreateRegistration(registerOptions, procedureUri));

                return registration.Register(operation, registerOptions);                
            }
        }

        private ProcedureRegistration CreateRegistration(RegisterOptions registerOptions, string procedureUri)
        {
            ProcedureRegistration result = new ProcedureRegistration(procedureUri, registerOptions);

            result.Empty += OnRegistrationEmpty;

            long registrationId = mRegistrationIdToRegistration.Add(result);

            result.RegistrationId = registrationId;

            OnRegistrationAdded(procedureUri);

            return result;
        }

        // These hooks will be changed in meta-api version
        protected virtual void OnRegistrationAdded(string procedureUri)
        {
        }

        protected virtual void OnRegistrationRemoved(string procedureUri)
        {
        }

        private void OnRegistrationEmpty(object sender, EventArgs e)
        {
            ProcedureRegistration registration = sender as ProcedureRegistration;

            if (!registration.HasOperations)
            {
                lock (mLock)
                {
                    if (!registration.HasOperations)
                    {
                        ProcedureRegistration removed;
                        mRegistrationIdToRegistration.TryRemove(registration.RegistrationId, out removed);
                        mProcedureToRegistration.TryRemove(registration.Procedure, out removed);
                        registration.Empty -= OnRegistrationEmpty;
                        OnRegistrationRemoved(registration.Procedure);
                    }
                }
            }
        }

        public bool Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            return InvokePattern(procedure,
                                 operation =>
                                     operation.Invoke(caller, formatter, details));
        }

        public bool Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            return InvokePattern(procedure,
                                 operation =>
                                     operation.Invoke(caller, formatter, details, arguments));
        }

        public bool Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
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

        private bool InvokePattern(string procedure, Action<IWampRpcOperation> invokeAction)
        {
            IWampRpcOperation operation = TryGetOperation(procedure);

            if (operation == null)
            {
                return false;
            }
            else
            {
                invokeAction(operation);

                return true;                
            }
        }

        private IWampRpcOperation TryGetOperation(string procedure)
        {
            return GetMatchingOperation(procedure);
        }

        protected IWampRpcOperation GetOperationByUri(string procedureUri)
        {
            ProcedureRegistration result;
            
            if (mProcedureToRegistration.TryGetValue(procedureUri, out result))
            {
                return result;
            }

            return null;
        }

        protected IEnumerable<IWampRpcOperation> Operations
        {
            get
            {
                return this.mProcedureToRegistration.Values;
            }
        }

        #region Abstract methods
        
        public abstract bool Handles(RegisterOptions options);

        protected abstract IWampRpcOperation GetMatchingOperation(string criteria);

        #endregion
    }
}