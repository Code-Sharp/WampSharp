using System;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    public class WampRealmProxy<TMessage> : IWampRealmProxy
    {
        private readonly IWampTopicContainerProxy mTopicContainer;
        private readonly IWampRpcOperationCatalogProxy mRpcCatalog;
        private readonly string mName;
        private readonly IWampServerProxy mProxy;
        private readonly IWampRealmServiceProvider mServices;

        public WampRealmProxy(string name, IWampServerProxy proxy, IWampBinding<TMessage> binding)
        {
            mName = name;
            mProxy = proxy;
            IWampFormatter<TMessage> formatter = binding.Formatter;
            mRpcCatalog = new WampRpcOperationCatalogProxy<TMessage>(proxy, formatter);
            mTopicContainer = new WampTopicContainerProxy<TMessage>(proxy, formatter);
            mServices = new WampRealmProxyServiceProvider(this);
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public IWampTopicContainerProxy TopicContainer
        {
            get
            {
                return mTopicContainer;
            }
        }

        public IWampRpcOperationCatalogProxy RpcCatalog
        {
            get
            {
                return mRpcCatalog;
            }
        }

        public IWampServerProxy Proxy
        {
            get { return mProxy; }
        }

        public IWampRealmServiceProvider Services
        {
            get { return mServices; }
        }

        public event EventHandler<WampSessionEventArgs> ConnectionEstablished;
        public event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}