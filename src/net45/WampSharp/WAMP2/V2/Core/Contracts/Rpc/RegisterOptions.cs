using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Register)]
    public class RegisterOptions : WampDetailsOptions
    {
        public RegisterOptions()
        {
        }

        public RegisterOptions(RegisterOptions other)
        {
            this.DiscloseCaller = other.DiscloseCaller;
        }

        [DataMember(Name = "disclose_caller")]
        public bool? DiscloseCaller { get; set; }

        [DataMember(Name = "invoke")]
        public string Invoke { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }
    }
}