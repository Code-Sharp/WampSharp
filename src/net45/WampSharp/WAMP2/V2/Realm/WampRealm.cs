using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealm : IWampRealm
    {
        private readonly string mName;
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampTopicContainer mTopicContainer;
        private readonly IWampRealmServiceProvider mServices;

        public WampRealm(string name, IWampRpcOperationCatalog catalog, IWampTopicContainer topicContainer)
        {
            mName = name;
            mCatalog = catalog;
            mTopicContainer = topicContainer;
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public IWampRpcOperationCatalog RpcCatalog
        {
            get
            {
                return mCatalog;
            }
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mTopicContainer;
            }
        }

        public IWampRealmServiceProvider Services
        {
            get
            {
                return mServices;
            }
        }
    }
}