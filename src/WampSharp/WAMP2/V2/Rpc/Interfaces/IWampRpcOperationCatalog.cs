namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationCatalog : IWampRpcOperationInvoker
    {
        void Register(IWampRpcOperation operation);

        void Unregister(IWampRpcOperation operation);

        // TODO: add methods for reflection :)
    }
}