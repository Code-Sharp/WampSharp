namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilder<TMessage, TClient>
    {
        TClient Create(IWampConnection<TMessage> connection);
    }
}