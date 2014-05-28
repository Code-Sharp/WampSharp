using WampSharp.Core.Serialization;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp.Cra
{
    public interface IWampCraServerBuilder<TMessage> : IWampServerBuilder<TMessage>
    {
        void InitializeAuthenticationBuilder(IWampFormatter<TMessage> formatter,
            IWampRpcMetadataCatalog rpcMetadataCatalog,
            IWampTopicContainerExtended<TMessage> topicContainer);
    }
}
