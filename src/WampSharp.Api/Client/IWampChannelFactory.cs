using WampSharp.Core.Listener;

namespace WampSharp.Api
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IWampConnection<TMessage> connection);
    }
}