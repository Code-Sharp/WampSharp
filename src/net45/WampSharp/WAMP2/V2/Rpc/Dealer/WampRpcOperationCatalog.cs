using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog : IWampRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, ProcedureRegistration> mProcedureToRegistration =
            new ConcurrentDictionary<string, ProcedureRegistration>();

        private readonly WampIdMapper<ProcedureRegistration> mRegistrationIdToRegistration =
            new WampIdMapper<ProcedureRegistration>();
        
        private readonly object mLock = new object();

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

            return result;
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
                    }
                }
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments,
                   argumentsKeywords);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details);
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details, string procedure, TMessage[] arguments)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments);
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details, string procedure, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation TryGetOperation(IWampRawRpcOperationRouterCallback caller, InvocationDetails details,
                                                  string procedure)
        {
            ProcedureRegistration registration;

            if (mProcedureToRegistration.TryGetValue(procedure, out registration))
            {
                return registration;
            }
            else
            {
                caller.NoProcedureRegistered(procedure);
                return null;
            }
        }

        private IWampRpcOperation<TMessage> CastOperation<TMessage>
            (IWampRpcOperation operation, IWampFormatter<TMessage> formatter)
        {
            IWampRpcOperation<TMessage> casted = operation as IWampRpcOperation<TMessage>;

            if (casted != null)
            {
                return casted;
            }
            else
            {
                return new CastedRpcOperation<TMessage>(operation, formatter);
            }
        }
    }
}