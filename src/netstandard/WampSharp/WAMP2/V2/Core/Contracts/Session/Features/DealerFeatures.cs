using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class DealerFeatures
    {
        [DataMember(Name = "caller_identification")]
        public bool? CallerIdentification { get; internal set; }

        [DataMember(Name = "pattern_based_registration")]
        public bool? PatternBasedRegistration { get; internal set; }

        [DataMember(Name = "session_meta_api")]
        public bool? SessionMetaApi { get; internal set; }

        [DataMember(Name = "registration_meta_api")]
        public bool? RegistrationMetaApi { get; internal set; }

        [DataMember(Name = "shared_registration")]
        public bool? SharedRegistration { get; internal set; }

        [DataMember(Name = "call_canceling")]
        public bool? CallCanceling { get; internal set; }

        [DataMember(Name = "progressive_call_results")]
        public bool? ProgressiveCallResults { get; internal set; }

        [DataMember(Name = "registration_revocation")]
        public bool? RegistrationRevocation { get; internal set; }

        [DataMember(Name = "testament_meta_api")]
        public bool? TestamentMetaApi { get; internal set; }
    }
}