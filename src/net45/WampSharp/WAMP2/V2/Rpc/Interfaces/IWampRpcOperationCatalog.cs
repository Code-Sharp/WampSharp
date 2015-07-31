using WampSharp.V2.Core.Contracts;

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
        /// <param name="options"></param>
        IWampRpcOperationRegistrationToken Register(IWampRpcOperation operation, RegisterOptions options);
    }
}