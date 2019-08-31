using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampOutgoingMessageHandler"/>
    /// that sends <see cref="WampMessage{TMessage}"/> to a given
    /// <see cref="IWampConnection{TMessage}"/> and receives from that connection
    /// messages which are handled by an <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampServerProxyHandler<TMessage> : IWampOutgoingMessageHandler
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampIncomingMessageHandler<TMessage> mIncomingHandler;

        /// <summary>
        /// Creates a new instance of <see cref="WampServerProxyHandler{TMessage}"/>.
        /// </summary>
        /// <param name="connection">The connection used to send and receieve <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="incomingHandler">The handler used to handle incoming messages.</param>
        public WampServerProxyHandler(IWampConnection<TMessage> connection,
                                      IWampIncomingMessageHandler<TMessage> incomingHandler)
        {
            mConnection = connection;
            mIncomingHandler = incomingHandler;

            mConnection.MessageArrived += MessageArrived;
        }

        private void MessageArrived(object sender, WampMessageArrivedEventArgs<TMessage> e)
        {
            OnMessageArrived(e.Message);
        }

        private void OnMessageArrived(WampMessage<TMessage> wampMessage)
        {
            mIncomingHandler.HandleMessage(wampMessage);
        }

        public void Handle(WampMessage<object> message)
        {
            mConnection.Send(message);
        }
    }
}