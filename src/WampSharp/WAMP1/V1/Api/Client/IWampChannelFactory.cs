using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.V1
{
    public interface IWampChannelFactory<TMessage>
    {
        IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection);
        IWampFormatter<TMessage> Formatter { get; }
    }
}