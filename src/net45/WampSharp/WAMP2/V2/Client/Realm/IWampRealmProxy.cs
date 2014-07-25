using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

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

        event EventHandler<WampSessionEventArgs> ConnectionEstablished;
        event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}