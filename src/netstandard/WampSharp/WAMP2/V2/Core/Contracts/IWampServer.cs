namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents all methods defined for a WAMP2 router.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampServer<TMessage> :
        IWampSessionServer<TMessage>,
        IWampDealer<TMessage>,
        IWampRpcInvocationCallback<TMessage>,
        IWampErrorCallback<TMessage>,
        IWampBroker<TMessage>
    {
    }
}