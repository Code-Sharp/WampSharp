using System;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealm : IWampRealm
    {
        private readonly string mName;
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampTopicContainer mTopicContainer;
        private IWampRealmServiceProvider mServices;

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

        public event EventHandler<WampSessionEventArgs> SessionCreated;
        public event EventHandler<WampSessionCloseEventArgs> SessionClosed;
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

        public IWampRealmServiceProvider Services
        {
            get
            {
                return mRealm.Services;
            }
        }

        public event EventHandler<WampSessionCloseEventArgs> SessionClosed
        {
            add { mRealm.SessionClosed += value; }
            remove { mRealm.SessionClosed -= value; }
        }

        public event EventHandler<WampSessionEventArgs> SessionCreated
        {
            add { mRealm.SessionCreated += value; }
            remove { mRealm.SessionCreated -= value; }
        }
    }
}