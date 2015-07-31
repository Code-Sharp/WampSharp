using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class InvocationDetailsExtended : InvocationDetails
    {
        public InvocationDetailsExtended()
        {
        }

        public InvocationDetailsExtended(InvocationDetailsExtended details) :
            base(details)
        {
            CallerOptions = details.CallerOptions;
        }

        [IgnoreDataMember]
        public CallOptions CallerOptions { get; set; }

        [IgnoreDataMember]
        public long? CallerSession { get; set; }

        [IgnoreDataMember]
        public string ProcedureUri { get; set; }
    }
}