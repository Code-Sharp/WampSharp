namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Represents a container for rpc procedures registered in a realm.
    /// </summary>
    public interface IWampRpcOperationCatalog : IWampRpcOperationInvoker
    {
        /// <summary>
        /// Registers a <see cref="IWampRpcOperation"/> to the realm.
        /// </summary>
        /// <param name="operation"></param>
        void Register(IWampRpcOperation operation);

        /// <summary>
        /// Unregisters a given <see cref="IWampRpcOperation"/> from the catalog.
        /// </summary>
        /// <param name="operation"></param>
        void Unregister(IWampRpcOperation operation);
    }
}