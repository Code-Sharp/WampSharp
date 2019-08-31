using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Welcome)]
    public class WelcomeDetails : WampDetailsOptions
    {
        [DataMember(Name = "authrole")]
        public string AuthenticationRole { get; set; }

        [DataMember(Name = "authmethod")]
        public string AuthenticationMethod { get; internal set; }

        [DataMember(Name = "authprovider")]
        public string AuthenticationProvider { get; set; }

        [DataMember(Name = "roles")]
        public RouterRoles Roles { get; internal set; }

        [DataMember(Name = "authid")]
        public string AuthenticationId { get; internal set; }
    }
}