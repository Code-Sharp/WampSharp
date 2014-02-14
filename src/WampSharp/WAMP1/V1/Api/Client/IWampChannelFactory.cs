using WampSharp.Core.Listener;

namespace WampSharp.V1
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection);
    }
}