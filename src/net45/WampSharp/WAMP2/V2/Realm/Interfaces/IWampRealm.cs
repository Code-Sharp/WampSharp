using System;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public interface IWampRealm
    {
        string Name { get; }

        IWampRpcOperationCatalog RpcCatalog { get; }

        IWampTopicContainer TopicContainer { get; }

        event EventHandler<WampSessionEventArgs> SessionCreated;
        event EventHandler<WampSessionEventArgs> SessionClosed;

        IWampRealmServiceProvider Services { get; }
    }

    public interface IWampRealm<TMessage> : IWampRealm
    {
        IWampServer<TMessage> Server { get; }
    }
}