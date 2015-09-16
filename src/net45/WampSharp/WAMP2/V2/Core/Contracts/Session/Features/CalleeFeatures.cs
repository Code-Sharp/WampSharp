using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class CalleeFeatures
    {
        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }
    }
}