namespace WampSharp.V2.Core.Contracts
{
    public interface IWampServerProxy : IWampServerProxy<object>
    {
         
    }

    public interface IWampServerProxy<TMessage> :
        IWampBrokerProxy<TMessage>, 
        IWampDealerProxy<TMessage>, 
        IWampSessionProxy<TMessage>,
        IWampError<TMessage>
    {
    }
}