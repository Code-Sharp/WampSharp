using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Register)]
    public class RegisterOptions : WampDetailsOptions
    {
        public RegisterOptions()
        {
        }

        public RegisterOptions(RegisterOptions other)
        {
            this.DiscloseCaller = other.DiscloseCaller;
            this.Match = other.Match;
            this.Invoke = other.Invoke;
            this.OriginalValue = other.OriginalValue;
        }

        /// <summary>
        /// If <see langword="true"/>, the (registering) callee requests to disclose the identity of callers whenever called.
        /// </summary>
        [DataMember(Name = "disclose_caller")]
        public bool? DiscloseCaller { get; set; }

        /// <summary>
        /// The procedure invocation policy to be used for the registration.
        /// (Mostly supported: <see cref="WampInvokePolicy"/> values: null/"single"/"first"/"last"/"random"/"roundrobin")
        /// </summary>
        [DataMember(Name = "invoke")]
        public string Invoke { get; set; }

        /// <summary>
        /// The procedure matching policy to be used for the registration.
        /// (Mostly supported: <see cref="WampMatchPattern"/> values: null/"exact"/"prefix"/"wildcard")
        /// </summary>
        [DataMember(Name = "match")]
        public string Match { get; set; }
    }
}