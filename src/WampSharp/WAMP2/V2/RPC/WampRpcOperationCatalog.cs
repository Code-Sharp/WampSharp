using System.Collections.Concurrent;

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
                // throw something to the poor WAMP callee.
            }
        }

        public void Unregister(IWampRpcOperation<TMessage> operation)
        {
            IWampRpcOperation<TMessage> result;

            if (!mProcedureToOperation.TryRemove(operation.Procedure, out result))
            {
                // throw something to the poor WAMP callee.
            }
        }

        public void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options);
            }
        }

        public void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure,
                           TMessage[] arguments)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options, arguments);
            }
        }

        public void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure,
                           TMessage[] arguments,
                           TMessage argumentsKeywords)
        {
            IWampRpcOperation<TMessage> operation = TryGetOperation(caller, options, procedure);

            if (operation != null)
            {
                operation.Invoke(caller, options, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation<TMessage> TryGetOperation(IWampRpcOperationCallback<TMessage> caller, TMessage options,
                                                            string procedure)
        {
            IWampRpcOperation<TMessage> operation;

            if (!mProcedureToOperation.TryGetValue(procedure, out operation))
            {
                // throw something to the poor WAMP callee.
                //caller.Error();
                return null;
            }
            else
            {
                return operation;
            }
        }
    }
}