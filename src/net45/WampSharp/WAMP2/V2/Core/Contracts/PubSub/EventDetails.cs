using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details for EVENT message.
    /// </summary>
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Event)]
    public class EventDetails : WampDetailsOptions
    {
        /// <summary>
        /// Gets or sets the publisher id of this publication.
        /// </summary>
        [DataMember(Name = "publisher")]
        public long? Publisher { get; set; }

        /// <summary>
        /// Gets or sets the topic of this publication.
        /// </summary>
        [DataMember(Name = "topic")]
        public string Topic { get; set; }
    }
}