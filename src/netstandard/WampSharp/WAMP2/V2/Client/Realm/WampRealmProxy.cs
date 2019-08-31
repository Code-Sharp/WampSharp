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
        private readonly WampSessionClient<TMessage> mMonitor;

        public WampRealmProxy(string name, IWampServerProxy proxy, IWampBinding<TMessage> binding, IWampClientAuthenticator authenticator)
        {
            Name = name;
            Proxy = proxy;
            IWampFormatter<TMessage> formatter = binding.Formatter;
            mMonitor = new WampSessionClient<TMessage>(this, formatter, authenticator);
            RpcCatalog = new WampRpcOperationCatalogProxy<TMessage>(proxy, formatter, mMonitor);
            TopicContainer = new WampTopicContainerProxy<TMessage>(proxy, formatter, mMonitor);
            Services = new WampRealmProxyServiceProvider(this);
            Authenticator = authenticator;
        }

        public string Name { get; }

        public IWampTopicContainerProxy TopicContainer { get; }

        public IWampRpcOperationCatalogProxy RpcCatalog { get; }

        public IWampServerProxy Proxy { get; }

        public IWampRealmServiceProvider Services { get; }

        public IWampClientConnectionMonitor Monitor => mMonitor;

        public IWampClientAuthenticator Authenticator { get; }
    }
}