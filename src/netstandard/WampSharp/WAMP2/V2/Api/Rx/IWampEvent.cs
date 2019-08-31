using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a event that can be published via a <see cref="IWampSubject"/>.
    /// </summary>
    public interface IWampEvent
    {
        /// <summary>
        /// The publication objects.
        /// </summary>
        PublishOptions Options { get; }
        
        /// <summary>
        /// The publication arguments.
        /// </summary>
        object[] Arguments { get; }
        
        /// <summary>
        /// The publication arguments keywords.
        /// </summary>
        IDictionary<string, object> ArgumentsKeywords { get; }
    }
}