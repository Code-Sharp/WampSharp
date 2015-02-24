using System;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    internal class WampServiceHostedRealm : IWampHostedRealm
    {
        private readonly IWampHostedRealm mUnderlyingRealm;
        private readonly IWampChannel mInternalChannel;

        public WampServiceHostedRealm(IWampHostedRealm underlyingRealm, IWampChannel internalChannel)
        {
            mUnderlyingRealm = underlyingRealm;
            mInternalChannel = internalChannel;
        }

        public string Name
        {
            get
            {
                return mUnderlyingRealm.Name;
            }
        }

        public IWampRpcOperationCatalog RpcCatalog
        {
            get
            {
                return mUnderlyingRealm.RpcCatalog;
            }
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mUnderlyingRealm.TopicContainer;
            }
        }

        public event EventHandler<WampSessionEventArgs> SessionCreated
        {
            add { mUnderlyingRealm.SessionCreated += value; }
            remove { mUnderlyingRealm.SessionCreated -= value; }
        }

        public event EventHandler<WampSessionCloseEventArgs> SessionClosed
        {
            add { mUnderlyingRealm.SessionClosed += value; }
            remove { mUnderlyingRealm.SessionClosed -= value; }
        }
        
        public IWampRealmServiceProvider Services
        {
            get
            {
                return mInternalChannel.RealmProxy.Services;
            }
        }
    }
}