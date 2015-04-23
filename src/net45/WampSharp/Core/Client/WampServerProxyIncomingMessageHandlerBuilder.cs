using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyIncomingMessageHandlerBuilder{TMessage,TRawClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    public class WampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> : IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="WampServerProxyIncomingMessageHandlerBuilder{TMessage,TRawClient}"/>.
        /// </summary>
        /// <param name="formatter">A <see cref="IWampFormatter{TMessage}"/> used
        /// in order to deserialize requests.</param>
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