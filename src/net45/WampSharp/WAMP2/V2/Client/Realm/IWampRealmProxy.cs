using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to a WAMP realm.
    /// </summary>
    public interface IWampRealmProxy
    {
        /// <summary>
        /// Gets this realm's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IWampTopicContainerProxy"/> associated with this
        /// realm proxy.
        /// </summary>
        IWampTopicContainerProxy TopicContainer { get; }

        /// <summary>
        /// Gets the <see cref="IWampRpcOperationCatalogProxy"/> associated with this
        /// realm proxy.
        /// </summary>
        IWampRpcOperationCatalogProxy RpcCatalog { get; }

        // Not sure this should be exposed.
        /// <summary>
        /// Gets a <see cref="IWampServerProxy"/> associated with this realm proxy.
        /// </summary>
        IWampServerProxy Proxy { get; }

        /// <summary>
        /// Gets the services for this realm proxy.
        /// </summary>
        IWampRealmServiceProvider Services { get; }

        /// <summary>
        /// Gets a connection monitor assoicated with this realm proxy.
        /// </summary>
        IWampClientConnectionMonitor Monitor { get; }
    }
}