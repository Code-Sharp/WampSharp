using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilder<TMessage>
    {
        IWampClient Create(IWampConnection<TMessage> connection);
    }
}