using System;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampProcedureRegistration : IWampRpcOperation
    {
        #region Core

        IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation,
                                                    RegisterOptions registerOptions);

        #endregion

        #region Maintenance

        event EventHandler Empty;

        bool HasOperations { get; }

        #endregion

        #region Metadata

        long RegistrationId { get; set; }

        RegisterOptions RegisterOptions { get; }

        event EventHandler<WampCalleeAddEventArgs> CalleeRegistering;
        event EventHandler<WampCalleeAddEventArgs> CalleeRegistered;
        event EventHandler<WampCalleeRemoveEventArgs> CalleeUnregistering;
        event EventHandler<WampCalleeRemoveEventArgs> CalleeUnregistered;

        #endregion
    }
}