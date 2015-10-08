using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class RemoteWampCalleeDetails
    {
        private readonly IWampCallee mCallee;
        private readonly string mProcedure;
        private readonly RegisterOptions mOptions;

        protected RemoteWampCalleeDetails(IWampCallee callee, string procedure, RegisterOptions options)
        {
            mCallee = callee;
            mProcedure = procedure;
            mOptions = options;
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

        public RegisterOptions Options
        {
            get
            {
                return mOptions;
            }
        }

        public IWampCallee Callee
        {
            get
            {
                return mCallee;
            }
        }

        public long SessionId
        {
            get
            {
                IWampClientProperties properties = mCallee as IWampClientProperties;
                return properties.Session;
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
                   ((string.Equals(mProcedure, other.mProcedure) &&
                     string.Equals(Options.Match, other.Options.Match)) ||
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