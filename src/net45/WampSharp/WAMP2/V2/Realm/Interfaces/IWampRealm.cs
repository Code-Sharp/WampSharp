using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public interface IWampRealm
    {
        string Name { get; }

        IWampRpcOperationCatalog RpcCatalog { get; }

        IWampTopicContainer TopicContainer { get; }
    }
}