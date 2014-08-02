using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// A default implementation of <see cref="IWampRealmProxy"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    internal class WampRealmProxy<TMessage> : IWampRealmProxy
    {
        private readonly IWampTopicContainerProxy mTopicContainer;
        private readonly IWampRpcOperationCatalogProxy mRpcCatalog;
        private readonly string mName;
        private readonly IWampServerProxy mProxy;
        private readonly IWampRealmServiceProvider mServices;
        private readonly WampSessionClient<TMessage> mMonitor;

        public WampRealmProxy(string name, IWampServerProxy proxy, IWampBinding<TMessage> binding)
        {
            mName = name;
            mProxy = proxy;
            IWampFormatter<TMessage> formatter = binding.Formatter;
            mRpcCatalog = new WampRpcOperationCatalogProxy<TMessage>(proxy, formatter);
            mTopicContainer = new WampTopicContainerProxy<TMessage>(proxy, formatter);
            mServices = new WampRealmProxyServiceProvider(this);
            mMonitor = new WampSessionClient<TMessage>(this, formatter);
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

        public IWampClientConnectionMonitor Monitor
        {
            get
            {
                return mMonitor;
            }
        }
    }
}