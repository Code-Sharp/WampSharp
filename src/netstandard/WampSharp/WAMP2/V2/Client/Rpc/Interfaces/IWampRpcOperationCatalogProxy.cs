using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to a <see cref="IWampRpcOperationCatalog"/>.
    /// </summary>
    public interface IWampRpcOperationCatalogProxy :
        IWampRpcOperationRegistrationProxy, IWampRpcOperationInvokerProxy
    {
    }
}