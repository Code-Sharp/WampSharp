using WampSharp.V2.Rpc;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampServer<TMessage> :
        IWampSessionServer<TMessage>,
        IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage>,
        IWampBroker<TMessage>
    {
    }
}