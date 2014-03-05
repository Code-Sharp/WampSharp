using System.Collections.Concurrent;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog<TMessage> : IWampRpcOperationCatalog<TMessage> where TMessage : class
    {
        private readonly ConcurrentDictionary<string, IWampRpcOperation<TMessage>> mProcedureToOperation =
            new ConcurrentDictionary<string, IWampRpcOperation<TMessage>>();

        public void Register(IWampRpcOperation<TMessage> operation)
        {
            if (!mProcedureToOperation.TryAdd(operation.Procedure, operation))
            {
                throw new WampException(WampErrors.ProcedureAlreadyExists, operation.Procedure);
            }
        }

        public void Unregister(IWampRpcOperation<TMessage> operation)
        {
            IWampRpcOperation<TMessage> result;

            if (!mProcedureToOperation.TryRemove(operation.Procedure, out result))
            {
                throw new WampException(WampErrors.NoSuchRegistration, operation.Procedure);
            }
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options);
            }
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure,
                           TMessage[] arguments)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options, arguments);
            }
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure,
                           TMessage[] arguments,
                           TMessage argumentsKeywords)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation<TMessage> TryGetOperation(IWampRpcOperationCallback caller, TMessage options,
                                                            string procedure)
        {
            IWampRpcOperation<TMessage> operation;

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
    }
}