using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Serialization;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp
{
    public interface IWampServerBuilder<TMessage>
    {
        IWampServer<TMessage> Build(IWampFormatter<TMessage> formatter, WampRpcMetadataCatalog rpcMetadataCatalog, IWampTopicContainerExtended<TMessage> topicContainer);
    }
}