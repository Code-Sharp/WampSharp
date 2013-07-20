using WampSharp.Core.Listener;

namespace WampSharp
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection);
    }
}