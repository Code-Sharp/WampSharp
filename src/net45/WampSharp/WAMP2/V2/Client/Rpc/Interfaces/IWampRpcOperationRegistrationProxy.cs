using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to a <see cref="IWampRpcOperationCatalog"/>'s
    /// registration methods.
    /// </summary>
    public interface IWampRpcOperationRegistrationProxy
    {
        /// <summary>
        /// Registers an operation to the realm.
        /// </summary>
        /// <param name="operation">The operation to register.</param>
        /// <param name="options">The options to register with.</param>
        /// <returns>A task that completes when registration is complete.</returns>
        Task<IAsyncDisposable> Register(IWampRpcOperation operation, RegisterOptions options);
    }
}