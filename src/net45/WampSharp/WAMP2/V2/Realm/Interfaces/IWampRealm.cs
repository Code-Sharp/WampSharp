using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Represents a WAMP realm. That is a domain, where uris are mapped
    /// to topics and procedures.
    /// </summary>
    public interface IWampRealm
    {
        /// <summary>
        /// Gets the realm's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IWampRpcOperationCatalog"/> associated with this realm.
        /// </summary>
        IWampRpcOperationCatalog RpcCatalog { get; }

        /// <summary>
        /// Gets the <see cref="IWampTopicContainer"/> associated with this realm.
        /// </summary>
        IWampTopicContainer TopicContainer { get; }
    }
}