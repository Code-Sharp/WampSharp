using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a raw form of an event received from a WAMP topic.
    /// </summary>
    public interface IWampSerializedEvent
    {
        /// <summary>
        /// Gets the publication id of this event.
        /// </summary>
        long PublicationId { get; }

        /// <summary>
        /// Gets the details associated with this event.
        /// </summary>
        EventDetails Details { get; }

        /// <summary>
        /// Gets the arguments of this event.
        /// </summary>
        ISerializedValue[] Arguments { get; }

        /// <summary>
        /// Gets the arguments keywords of this event.
        /// </summary>
        IDictionary<string, ISerializedValue> ArgumentsKeywords { get; }
    }
}