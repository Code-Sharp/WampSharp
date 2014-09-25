using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details for EVENT message.
    /// </summary>
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class EventDetails : WampDetailsOptions
    {
        /// <summary>
        /// Gets or sets the publisher id of this publication.
        /// </summary>
        [PropertyName("publisher")]
        public long? Publisher { get; set; }
    }
}