using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampOutgoingMessageHandlerBuilder{TMessage}"/>
    /// using <see cref="WampOutgoingMessageHandler{TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampOutgoingMessageHandlerBuilder<TMessage> : IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        public IWampOutgoingMessageHandler Build(IWampConnection<TMessage> connection)
        {
            return new WampOutgoingMessageHandler<TMessage>(connection);
        }
    }
}