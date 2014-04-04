using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Client
{
    public class WampRealmProxy<TMessage> : IWampRealmProxy
    {
        private IWampTopicContainerProxy mTopicContainer;
        private readonly IWampRpcOperationCatalogProxy mRpcCatalog;
        private readonly string mName;

        public WampRealmProxy(string name, IWampServerProxy proxy, IWampBinding<TMessage> binding)
        {
            mName = name;
            IWampFormatter<TMessage> formatter = binding.Formatter;
            mRpcCatalog = new WampRpcOperationCatalogProxy<TMessage>(proxy, formatter);
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
    }
}