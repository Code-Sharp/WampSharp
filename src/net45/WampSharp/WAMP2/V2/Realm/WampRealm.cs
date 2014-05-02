using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealm : IWampRealm
    {
        private readonly string mName;
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampTopicContainer mTopicContainer;

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
    }

    public class WampRealm<TMessage> : IWampRealm<TMessage>
    {
        private readonly IWampServer<TMessage> mServer;
        private readonly IWampRealm mRealm;

        public WampRealm(IWampRealm realm,
                         IWampSessionServer<TMessage> session,
                         IWampEventSerializer<TMessage> eventSerializer,
                         IWampBinding<TMessage> binding)
        {
            mRealm = realm;

            IWampRpcServer<TMessage> dealer =
                new WampRpcServer<TMessage>(realm.RpcCatalog, binding);

            IWampPubSubServer<TMessage> broker =
                new WampPubSubServer<TMessage>(realm.TopicContainer,
                                               eventSerializer,
                                               binding);

            mServer = new WampServer<TMessage>(session, dealer, broker);
        }


        public IWampServer<TMessage> Server
        {
            get
            {
                return mServer;
            }
        }

        public string Name
        {
            get
            {
                return mRealm.Name;
            }
        }

        public IWampRpcOperationCatalog RpcCatalog
        {
            get
            {
                return mRealm.RpcCatalog;
            }
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mRealm.TopicContainer;
            }
        }
    }
}