using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public interface IWampRealmProxy
    {
        string Name { get; }

        IWampTopicContainerProxy TopicContainer { get; }

        IWampRpcOperationCatalogProxy RpcCatalog { get; }

        // Not sure this should be exposed.
        IWampServerProxy Proxy { get; }

        IWampRealmServiceProvider Services { get; }

        IWampClientConnectionMonitor Monitor { get; }
    }
}