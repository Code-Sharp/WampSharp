namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilderFactory<TMessage, TClient>
    {
        IWampClientBuilder<TMessage, TClient> GetClientBuilder(IWampClientContainer<TMessage, TClient> container);
    }
}