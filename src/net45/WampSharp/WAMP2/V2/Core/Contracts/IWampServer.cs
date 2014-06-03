using WampSharp.V2.Rpc;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents all methods defined for a WAMP2 router.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampServer<TMessage> :
        IWampSessionServer<TMessage>,
        IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage>,
        IWampBroker<TMessage>
    {
    }
}