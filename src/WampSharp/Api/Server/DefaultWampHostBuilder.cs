using WampSharp.Auxiliary.Server;
using WampSharp.Core.Contracts.V1;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp
{
    public class DefaultWampHostBuilder<TMessage> : IWampHostBuilder<TMessage>
    {
        public virtual IWampRpcServer<TMessage> BuildRpcServer(Core.Serialization.IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog rpcMetadataCatalog)
        {
            return new WampRpcServer<TMessage>(formatter, rpcMetadataCatalog);
        }

        public virtual IWampAuxiliaryServer BuildAuxiliaryServer()
        {
            return new WampAuxiliaryServer();
        }

        public virtual IWampPubSubServer<TMessage> BuildPubSubServer(PubSub.Server.IWampTopicContainerExtended<TMessage> topicContainerExtended)
        {
            return new WampPubSubServer<TMessage>(topicContainerExtended);
        }

        public virtual IWampServer<TMessage> BuildWampServer(IWampRpcServer<TMessage> rpcServer, IWampPubSubServer<TMessage> pubSubServer, IWampAuxiliaryServer auxiliaryServer)
        {
            return new DefaultWampServer<TMessage>(rpcServer, pubSubServer, auxiliaryServer);
        }
    }
}
