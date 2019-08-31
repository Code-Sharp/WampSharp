namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a proxy to a WAMP2 router.
    /// </summary>
    public interface IWampServerProxy : IWampServerProxy<object>
    {
    }

    /// <summary>
    /// Represents a proxy to a WAMP2 router.
    /// </summary>
    public interface IWampServerProxy<TMessage> :
        IWampBrokerProxy<TMessage>, 
        IWampDealerProxy<TMessage>, 
        IWampSessionProxy,
        IWampError<TMessage>
    {
    }
}