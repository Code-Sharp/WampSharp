using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;
using WampSharp.V2.MetaApi;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Hello)]
    public class HelloDetails : WampDetailsOptions
    {
        /// <summary>
        /// Gets the announced WAMP roles.
        /// </summary>
        [DataMember(Name = "roles")]
        public ClientRoles Roles { get; internal set; }

        /// <summary>
        /// Gets the announced authentication methods.
        /// </summary>
        [DataMember(Name = "authmethods")]
        public string[] AuthenticationMethods { get; internal set; }

        /// <summary>
        /// Gets the announced authentication ID.
        /// </summary>
        [DataMember(Name = "authid")]
        public string AuthenticationId { get; internal set; }

        /// <summary>
        /// Gets the transport details associated with this client.
        /// </summary>
        [IgnoreDataMember]
        public WampTransportDetails TransportDetails { get; internal set; }
    }
}