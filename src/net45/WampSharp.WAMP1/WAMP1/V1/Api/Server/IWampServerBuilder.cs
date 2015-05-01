using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1
{
    public interface IWampServerBuilder<TMessage>
    {
        IWampServer<TMessage> Build(IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog rpcMetadataCatalog, IWampTopicContainerExtended<TMessage> topicContainer);
    }
}