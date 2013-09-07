using System.Collections.Generic;

namespace WampSharp.Rpc.Server
{
    /// <summary>
    /// Represents hosted rpc service metadata.
    /// </summary>
    public interface IWampRpcMetadata
    {
        /// <summary>
        /// Gets the methods of the current rpc service.
        /// </summary>
        /// <returns>The the methods of the current rpc service.</returns>
        IEnumerable<IWampRpcMethod> GetServiceMethods();
    }
}