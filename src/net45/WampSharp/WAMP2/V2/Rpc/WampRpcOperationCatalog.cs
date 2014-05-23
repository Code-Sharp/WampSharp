using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog : IWampRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, IWampRpcOperation> mProcedureToOperation =
            new ConcurrentDictionary<string, IWampRpcOperation>();

        public void Register(IWampRpcOperation operation)
        {
            if (!mProcedureToOperation.TryAdd(operation.Procedure, operation))
            {
                string registerError = 
                    string.Format("register for already registered procedure URI '{0}'", operation.Procedure);

                throw new WampException(WampErrors.ProcedureAlreadyExists,
                                        registerError);
            }
        }

        public void Unregister(IWampRpcOperation operation)
        {
            IWampRpcOperation result;

            if (operation == null)
            {
                throw new WampException(WampErrors.NoSuchRegistration);
            }

            if (!mProcedureToOperation.TryRemove(operation.Procedure, out result))
            {
                string registrationError = string.Format("no procedure '{0}' registered", operation.Procedure);
                
                throw new WampException(WampErrors.NoSuchRegistration, registrationError);
            }
        }

        public void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure)
        {
            IWampRpcOperation operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted = 
                    CastOperation(operation, formatter);

                casted.Invoke(caller, options);
            }
        }

        public void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure,
                                     TMessage[] arguments)
        {
            IWampRpcOperation operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, options, arguments);
            }
        }

        public void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure,
                                     TMessage[] arguments, TMessage argumentsKeywords)
        {
            IWampRpcOperation operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, options, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation TryGetOperation(IWampRpcOperationCallback caller, object options,
                                                  string procedure)
        {
            IWampRpcOperation operation;

            if (!mProcedureToOperation.TryGetValue(procedure, out operation))
            {
                caller.Error(procedure, WampErrors.NoSuchProcedure);
                return null;
            }
            else
            {
                return operation;
            }
        }

        private IWampRpcOperation<TMessage> CastOperation<TMessage>(IWampRpcOperation operation, IWampFormatter<TMessage> formatter)
            
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