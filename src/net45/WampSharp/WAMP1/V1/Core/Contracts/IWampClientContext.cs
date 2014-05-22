using System.Collections.Generic;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Represents a property bag interface for <see cref="IWampClient"/>,
    /// so properties can be attached easily to a client proxy.
    /// </summary>
    /// TODO: think about how to avoid this mechanism in WAMPv2.
    public interface IWampClientContext
    {
        /// <summary>
        /// Gets a key-value store of properties of the current client.
        /// </summary>
        IDictionary<string, object> ClientContext { get; }
    }
}