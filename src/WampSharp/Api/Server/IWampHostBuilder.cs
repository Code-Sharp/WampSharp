using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Auxiliary.Server;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Serialization;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp
{
    public interface IWampHostBuilder<TMessage>
    {
        IWampRpcServer<TMessage> BuildRpcServer(IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog rpcMetadataCatalog);

        IWampAuxiliaryServer BuildAuxiliaryServer();

        IWampPubSubServer<TMessage> BuildPubSubServer(IWampTopicContainerExtended<TMessage> topicContainerExtended);

        IWampServer<TMessage> BuildWampServer(IWampRpcServer<TMessage> rpcServer, IWampPubSubServer<TMessage> pubSubServer, IWampAuxiliaryServer auxiliaryServer);
    }
}
