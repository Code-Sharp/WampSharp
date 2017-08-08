using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class CallerFeatures
    {
        [DataMember(Name = "caller_identification")]
        public bool? CallerIdentification { get; internal set; }

        [DataMember(Name = "call_canceling")]
        public bool? CallCanceling { get; internal set; }

        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }
    }
}