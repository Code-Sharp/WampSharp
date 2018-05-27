using System;

namespace WampSharp.V2.Rpc
{
    public class WampProcedureRegisterEventArgs : EventArgs
    {
        public WampProcedureRegisterEventArgs(IWampProcedureRegistration registration)
        {
            Registration = registration;
        }

        public IWampProcedureRegistration Registration { get; }
    }
}