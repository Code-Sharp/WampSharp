using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class WampSessionDetails
    {
        /// <summary>
        /// Gets or sets the realm that the session is attached to
        /// </summary>
        [DataMember(Name = "realm")]
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the provider that performed the authentication of the session 
        /// </summary>
        [DataMember(Name = "authprovider")]
        public object AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the authentication ID of the session
        /// </summary>
        [DataMember(Name = "authid")]
        public string AuthId { get; set; }

        /// <summary>
        /// Gets or sets the authentication role of the session
        /// </summary>
        [DataMember(Name = "authrole")]
        public string AuthRole { get; set; }

        /// <summary>
        /// Gets or sets the authentication method that was used for authentication of the session
        /// </summary>
        [DataMember(Name = "authmethod")]
        public string AuthMethod { get; set; }

        /// <summary>
        /// Gets or sets the session ID of the session.
        /// </summary>
        [DataMember(Name = "session")]
        public long Session { get; set; }

        /// <summary>
        /// Gets or sets implementation defined information about the WAMP transport 
        /// the session is running over.
        /// </summary>
        [DataMember(Name = "transport")]
        public WampTransportDetails TransportDetails { get; set; }
    }
}