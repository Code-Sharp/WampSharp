using System;

namespace WampSharp.V2.Rpc
{
    public class WampProcedureRegisterEventArgs : EventArgs
    {
        private readonly IWampProcedureRegistration mRegistration;

        public WampProcedureRegisterEventArgs(IWampProcedureRegistration registration)
        {
            mRegistration = registration;
        }

        public IWampProcedureRegistration Registration
        {
            get
            {
                return mRegistration;
            }
        }
    }
}