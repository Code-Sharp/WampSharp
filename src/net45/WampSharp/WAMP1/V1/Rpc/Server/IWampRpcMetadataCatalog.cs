using System.Collections.Generic;

namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// Represents a catalog of hosted rpc services.
    /// </summary>
    public interface IWampRpcMetadataCatalog
    {
        /// <summary>
        /// Registers a new rpc service to the catalog
        /// given its metadata.
        /// </summary>
        /// <param name="metadata">The given service's metadata.</param>
        void Register(IWampRpcMetadata metadata);

        /// <summary>
        /// Unregisters a rpc method from the catalog
        /// </summary>
        /// <param name="method">The given method to unregister.</param>
        /// <returns>A value indicating whether the method was removed 
        /// successfully.</returns>
        bool Unregister(IWampRpcMethod method);

        /// <summary>
        /// Maps a given proc uri to its corresponding metadata.
        /// </summary>
        /// <param name="procUri">The given proc uri.</param>
        /// <returns>The corresponding metadata.</returns>
        IWampRpcMethod ResolveMethodByProcUri(string procUri);

        /// <summary>
        /// Gets all registered RPC methods.
        /// </summary>
        /// <returns>All registered RPC methods.</returns>
        IEnumerable<IWampRpcMethod> GetAllRpcMethods();
    }
}
