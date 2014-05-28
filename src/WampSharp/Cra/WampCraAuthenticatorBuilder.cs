using WampSharp.Core.Serialization;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp.Cra
{
    public abstract class WampCraAuthenticaticatorBuilder<TMessage>
    {
        private IWampFormatter<TMessage> mFormatter;
        private IWampRpcMetadataCatalog mRpcMetadataCatalog;
        private IWampTopicContainerExtended<TMessage> mTopicContainer;

        public void Initialize(IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog rpcMetadataCatalog, IWampTopicContainerExtended<TMessage> topicContainer)
        {
            mFormatter = formatter;
            mRpcMetadataCatalog = rpcMetadataCatalog;
            mTopicContainer = topicContainer;
        }

        protected IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mFormatter;
            }
        }

        protected IWampRpcMetadataCatalog RpcMetadataCatalog
        {
            get
            {
                return mRpcMetadataCatalog;
            }
        }

        protected IWampTopicContainerExtended<TMessage> TopicContainer
        {
            get
            {
                return mTopicContainer;
            }
        }

        public bool IsValid
        {
            get
            {
                return (mFormatter != null) && 
                    (mRpcMetadataCatalog != null) && 
                    (mTopicContainer != null);
            }
        }

        public abstract WampCraAuthenticator<TMessage> BuildAuthenticator(string clientSessionId);
    }
}
