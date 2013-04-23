namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilderFactory<TConnection>
    {
        IWampClientBuilder<TConnection> GetClientBuilder(IWampClientContainer<TConnection> container);
    }
}