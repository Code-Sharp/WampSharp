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
            this.OriginalValue = options.OriginalValue;
        }

        /// <summary>
        /// If <see cref="bool.True"/>, acknowledge the publication with a success or error response.
        /// </summary>
        [DataMember(Name = "acknowledge")]
        public bool? Acknowledge { get; set; }

        /// <summary>
        /// If <see cref="bool.True"/>, exclude the publisher from receiving the event, even if he is subscribed (and eligible).
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
        /// If <see cref="bool.True"/>, request to disclose the publisher of this event to subscribers.
        /// </summary>
        [DataMember(Name = "disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}