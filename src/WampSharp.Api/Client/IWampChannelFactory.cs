using WampSharp.Core.Listener;

namespace WampSharp.Api
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection);
    }
}