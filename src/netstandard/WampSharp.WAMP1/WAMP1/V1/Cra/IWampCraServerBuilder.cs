using WampSharp.Core.Serialization;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    public interface IWampCraServerBuilder<TMessage> : IWampServerBuilder<TMessage>
    {
        void InitializeAuthenticationBuilder(IWampFormatter<TMessage> formatter,
            IWampRpcMetadataCatalog rpcMetadataCatalog,
            IWampTopicContainerExtended<TMessage> topicContainer);
    }
}
