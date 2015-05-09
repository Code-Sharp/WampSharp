using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class RemoteWampCalleeDetails
    {
        private readonly IWampCallee mCallee;
        private readonly string mProcedure;

        protected RemoteWampCalleeDetails(IWampCallee callee, string procedure)
        {
            mCallee = callee;
            mProcedure = procedure;
        }

        public RemoteWampCalleeDetails(IWampCallee callee, long registrationId)
        {
            mCallee = callee;
            RegistrationId = registrationId;
        }

        public string Procedure
        {
            get
            {
                return mProcedure;
            }
        }

        public long RegistrationId
        {
            get;
            set;
        }

        public IWampCallee Callee
        {
            get
            {
                return mCallee;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (mCallee != null ? mCallee.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(RemoteWampCalleeDetails other)
        {
            return Equals(mCallee, other.mCallee) &&
                   (string.Equals(mProcedure, other.mProcedure) ||
                    RegistrationId == other.RegistrationId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            RemoteWampCalleeDetails casted = obj as RemoteWampCalleeDetails;
            
            if (casted == null)
            {
                return false;
            }
            
            return Equals(casted);
        }

        public static bool operator ==(RemoteWampCalleeDetails left, RemoteWampCalleeDetails right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RemoteWampCalleeDetails left, RemoteWampCalleeDetails right)
        {
            return !Equals(left, right);
        }
    }
}