using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Call)]
    public class CallOptions : WampDetailsOptions
    {
        public CallOptions()
        {
        }

        public CallOptions(CallOptions other)
        {
            TimeoutMili = other.TimeoutMili;
            ReceiveProgress = other.ReceiveProgress;
            DiscloseMe = other.DiscloseMe;
            OriginalValue = other.OriginalValue;
        }

        [IgnoreDataMember]
        [DataMember(Name = "timeout")]
        internal int? TimeoutMili { get; set; }

        /// <summary>
        /// If <see langword="true"/>, indicates that the caller wants to receive progressive call results.
        /// </summary>
        [DataMember(Name = "receive_progress")]
        public bool? ReceiveProgress { get; set; }

        /// <summary>
        /// If <see langword="true"/>, the caller requests to disclose itself to the callee.
        /// </summary>
        [DataMember(Name = "disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}