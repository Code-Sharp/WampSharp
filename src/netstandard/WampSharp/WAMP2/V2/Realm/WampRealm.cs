using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealm : IWampRealm
    {
        private readonly IWampRealmServiceProvider mServices;

        public WampRealm(string name, IWampRpcOperationCatalog catalog, IWampTopicContainer topicContainer)
        {
            Name = name;
            RpcCatalog = catalog;
            TopicContainer = topicContainer;
        }

        public string Name { get; }

        public IWampRpcOperationCatalog RpcCatalog { get; }

        public IWampTopicContainer TopicContainer { get; }

        public IWampRealmServiceProvider Services => mServices;
    }
}