namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationCatalog<TMessage> : IWampRpcOperationInvoker<TMessage>
        where TMessage : class
    {
        void Register(IWampRpcOperation<TMessage> operation);

        void Unregister(IWampRpcOperation<TMessage> operation);

        // TODO: add methods for reflection :)
    }
}