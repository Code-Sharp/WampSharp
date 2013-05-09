using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Client
{
    public class WampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> : IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampServerProxyIncomingMessageHandlerBuilder(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public IWampIncomingMessageHandler<TMessage> Build(TRawClient client, IWampConnection<TMessage> connection)
        {
            // No dependency injection here.
            return new WampIncomingMessageHandler<TMessage, object>
                (new WampRequestMapper<TMessage>(client.GetType(), mFormatter),
                 new WampMethodBuilder<TMessage, object>(client, mFormatter));
        }
    }
}