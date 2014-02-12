using WampSharp.Core.Listener;

namespace WampSharp.V1.Api.Client
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection);
    }
}