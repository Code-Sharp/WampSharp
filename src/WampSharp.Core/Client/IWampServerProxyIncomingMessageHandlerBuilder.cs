using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient>
    {
        IWampIncomingMessageHandler<TMessage> Build(TRawClient client, IWampConnection<TMessage> connection);
    }
}