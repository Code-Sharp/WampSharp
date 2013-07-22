using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampOutgoingMessageHandler{TMessage}"/>
    /// that dispatches messages to an <see cref="IWampConnection{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampOutgoingMessageHandler<TMessage> : IWampOutgoingMessageHandler<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;

        /// <summary>
        /// Creates a new instance of <see cref="WampOutgoingMessageHandler{TMessage}"/>
        /// using a given <see cref="IWampConnection{TMessage}"/>
        /// </summary>
        /// <param name="connection">The given WAMP connection.</param>
        public WampOutgoingMessageHandler(IWampConnection<TMessage> connection)
        {
            mConnection = connection;
        }

        public void Handle(WampMessage<TMessage> message)
        {
            mConnection.OnNext(message);
        }
    }
}