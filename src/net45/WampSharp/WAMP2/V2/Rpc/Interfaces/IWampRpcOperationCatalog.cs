using System;
using WampSharp.V2.Core;
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
        IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions options);

        /// <summary>
        /// Occurs when a procedure registration is added.
        /// </summary>
        event EventHandler<WampProcedureRegisterEventArgs> RegistrationAdded;

        /// <summary>
        /// Occurs when a procedure registration is removed.
        /// </summary>
        event EventHandler<WampProcedureRegisterEventArgs> RegistrationRemoved;

        /// <summary>
        /// Gets the best match for the given criteria.
        /// </summary>
        /// <param name="criteria">The given criteria.</param>
        /// <returns>The best match for the given criteria.</returns>
        IWampRpcOperation GetMatchingOperation(string criteria);
    }
}