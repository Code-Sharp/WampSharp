using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealm<TMessage> : IWampRealm<TMessage> where TMessage : class
    {
        private readonly string mName;
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampTopicContainer mTopicContainer;
        private readonly IWampServer<TMessage> mServer;

        public WampRealm(string name,
                         IWampRpcOperationCatalog catalog,
                         IWampTopicContainer topicContainer,
                         IWampSessionServer<TMessage> session,
                         IWampEventSerializer<TMessage> eventSerializer,
                         IWampBinding<TMessage> binding)
        {
            mCatalog = catalog;
            mTopicContainer = topicContainer;
            mName = name;

            IWampRpcServer<TMessage> dealer =
                new WampRpcServer<TMessage>(catalog, binding);

            IWampPubSubServer<TMessage> broker =
                new WampPubSubServer<TMessage>(topicContainer,
                                               eventSerializer,
                                               binding);

            mServer = new WampServer<TMessage>(session, dealer, broker);
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

        public IWampServer<TMessage> Server
        {
            get
            {
                return mServer;
            }
        }
    }
}