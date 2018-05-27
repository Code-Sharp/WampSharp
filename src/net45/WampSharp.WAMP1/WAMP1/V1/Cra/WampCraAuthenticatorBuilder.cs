using WampSharp.Core.Serialization;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    public abstract class WampCraAuthenticaticatorBuilder<TMessage>
    {
        private IWampTopicContainerExtended<TMessage> mTopicContainer;

        public void Initialize(IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog rpcMetadataCatalog, IWampTopicContainerExtended<TMessage> topicContainer)
        {
            Formatter = formatter;
            RpcMetadataCatalog = rpcMetadataCatalog;
            mTopicContainer = topicContainer;
        }

        protected IWampFormatter<TMessage> Formatter { get; private set; }

        protected IWampRpcMetadataCatalog RpcMetadataCatalog { get; private set; }

        protected IWampTopicContainerExtended<TMessage> TopicContainer => mTopicContainer;

        public bool IsValid => (Formatter != null) && 
                               (RpcMetadataCatalog != null) && 
                               (mTopicContainer != null);

        public abstract WampCraAuthenticator<TMessage> BuildAuthenticator(string clientSessionId);
    }
}
