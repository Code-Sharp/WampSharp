using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyIncomingMessageHandlerBuilder<TMessage>
    {
        IWampIncomingMessageHandler<TMessage> Build(IWampClient<TMessage> client, IWampConnection<TMessage> connection);
    }

    public class WampServerProxyIncomingMessageHandlerBuilder<TMessage> : IWampServerProxyIncomingMessageHandlerBuilder<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampServerProxyIncomingMessageHandlerBuilder(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public IWampIncomingMessageHandler<TMessage> Build(IWampClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            // No dependency injection here.
            return new WampIncomingMessageHandler<TMessage>
                (new WampRequestMapper<TMessage>(client.GetType(), mFormatter),
                 new WampMethodBuilder<TMessage>(client, mFormatter));
        }
    }
}