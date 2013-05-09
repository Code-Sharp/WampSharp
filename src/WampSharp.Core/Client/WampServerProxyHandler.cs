using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyHandler<TMessage> : IWampOutgoingMessageHandler<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampIncomingMessageHandler<TMessage> mIncomingHandler;

        public WampServerProxyHandler(IWampConnection<TMessage> connection,
                                      IWampIncomingMessageHandler<TMessage> incomingHandler)
        {
            mConnection = connection;
            mIncomingHandler = incomingHandler;

            mConnection.Subscribe(x => OnMessageArrived(x),
                                  x => OnError(x),
                                  () => OnConnectionClosed());
        }

        private void OnConnectionClosed()
        {
            // Not sure what to do.
        }

        private void OnError(Exception exception)
        {
            // Not sure what to do.
        }

        private void OnMessageArrived(WampMessage<TMessage> wampMessage)
        {
            mIncomingHandler.HandleMessage(wampMessage);
        }

        public void Handle(WampMessage<TMessage> message)
        {
            mConnection.OnNext(message);
        }
    }
}