using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog : IWampRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, IWampRpcOperation> mProcedureToOperation =
            new ConcurrentDictionary<string, IWampRpcOperation>();

        private readonly Dictionary<string, object> mEmptyDetails = new Dictionary<string, object>();

        private readonly IWampFormatter<object> ObjectFormatter = WampObjectFormatter.Value;

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

        public void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure);
        }

        public void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                                     string procedure, TMessage[] arguments)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments);
        }

        public void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                                     string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments,
                   argumentsKeywords);
        }

        public void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted = 
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details);
            }
        }

        public void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments);
            }
        }

        public void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation TryGetOperation(IWampRouterRawRpcOperationCallback caller, object options,
                                                  string procedure)
        {
            IWampRpcOperation operation;

            if (!mProcedureToOperation.TryGetValue(procedure, out operation))
            {
                string errorMessage = string.Format("no procedure '{0}' registered", procedure);

                caller.Error(ObjectFormatter,
                             mEmptyDetails,
                             WampErrors.NoSuchProcedure,
                             new object[] {errorMessage});
                
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