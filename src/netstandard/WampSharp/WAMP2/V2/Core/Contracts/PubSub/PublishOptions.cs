using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class PublishOptions : WampDetailsOptions
    {
        public PublishOptions()
        {
        }

        protected PublishOptions(PublishOptions options)
        {
            this.Acknowledge = options.Acknowledge;
            this.ExcludeMe = options.ExcludeMe;
            this.Exclude = options.Exclude;
            this.Eligible = options.Eligible;
            this.DiscloseMe = options.DiscloseMe;
            this.Retain = options.Retain;
            this.EligibleAuthenticationIds = options.EligibleAuthenticationIds;
            this.EligibleAuthenticationRoles = options.EligibleAuthenticationRoles;
            this.ExcludeAuthenticationIds = options.ExcludeAuthenticationIds;
            this.ExcludeAuthenticationRoles= options.ExcludeAuthenticationRoles;
            this.OriginalValue = options.OriginalValue;
        }

        /// <summary>
        /// If <see langword="true"/>, acknowledge the publication with a success or error response.
        /// </summary>
        [DataMember(Name = "acknowledge")]
        public bool? Acknowledge { get; set; }

        /// <summary>
        /// If <see langword="true"/>, exclude the publisher from receiving the event, even if he is subscribed (and eligible).
        /// </summary>
        [DataMember(Name = "exclude_me")]
        public bool? ExcludeMe { get; set; }

        /// <summary>
        /// List of WAMP session IDs to exclude from receiving this event.
        /// </summary>
        [DataMember(Name = "exclude")]
        public long[] Exclude { get; set; }

        /// <summary>
        /// List of WAMP session IDs eligible to receive this event.
        /// </summary>
        [DataMember(Name = "eligible")]
        public long[] Eligible { get; set; }

        /// <summary>
        /// If <see langword="true"/>, request to disclose the publisher of this event to subscribers.
        /// </summary>
        [DataMember(Name = "disclose_me")]
        public bool? DiscloseMe { get; set; }

        /// <summary>
        /// If <see langword="true"/>, request the broker retain this event.
        /// </summary>
        [DataMember(Name = "retain")]
        public bool? Retain { get; set; }

        /// <summary>
        /// List of WAMP authids eligible to receive this event.
        /// </summary>
        [DataMember(Name = "eligible_authid")]
        public string[] EligibleAuthenticationIds { get; set; }

        /// <summary>
        /// List of WAMP authroles eligible to receive this event.
        /// </summary>
        [DataMember(Name = "eligible_authrole")]
        public string[] EligibleAuthenticationRoles { get; set; }

        /// <summary>
        /// List of WAMP authids to exclude from receiving this event.
        /// </summary>
        [DataMember(Name = "exclude_authid")]
        public string[] ExcludeAuthenticationIds { get; set; }

        /// <summary>
        /// List of WAMP authroles to exclude from receiving this event.
        /// </summary>
        [DataMember(Name = "exclude_authrole")]
        public string[] ExcludeAuthenticationRoles { get; set; }
    }
}