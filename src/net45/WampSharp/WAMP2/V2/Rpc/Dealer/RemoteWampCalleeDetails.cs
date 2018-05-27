using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class RemoteWampCalleeDetails
    {
        private readonly RegisterOptions mOptions;

        protected RemoteWampCalleeDetails(IWampCallee callee, string procedure, RegisterOptions options)
        {
            Callee = callee;
            Procedure = procedure;
            mOptions = options;
        }

        public RemoteWampCalleeDetails(IWampCallee callee, long registrationId)
        {
            Callee = callee;
            RegistrationId = registrationId;
        }

        public string Procedure { get; }

        public long RegistrationId
        {
            get;
            set;
        }

        public RegisterOptions Options => mOptions;

        public IWampCallee Callee { get; }

        public long SessionId
        {
            get
            {
                IWampClientProperties properties = Callee as IWampClientProperties;
                return properties.Session;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Callee != null ? Callee.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(RemoteWampCalleeDetails other)
        {
            return Equals(Callee, other.Callee) &&
                   ((string.Equals(Procedure, other.Procedure) &&
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