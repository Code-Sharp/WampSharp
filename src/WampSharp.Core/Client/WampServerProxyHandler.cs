using System;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyHandlerBuilder<TMessage> : IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        private readonly IWampIncomingMessageHandler<TMessage> mIncomingHandler;

        public WampServerProxyHandlerBuilder(IWampIncomingMessageHandler<TMessage> incomingHandler)
        {
            mIncomingHandler = incomingHandler;
        }

        public IWampOutgoingMessageHandler<TMessage> Build(IWampConnection<TMessage> connection)
        {
            return new WampServerProxyHandler<TMessage>(connection, mIncomingHandler);
        }
    }

    public class WampServerProxyHandler<TMessage> : IWampOutgoingMessageHandler<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampIncomingMessageHandler<TMessage> mIncomingHandler;
        private IWampClient mClient;

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
            mIncomingHandler.HandleMessage(mClient, wampMessage);
        }

        public void Handle(IWampClient client, WampMessage<TMessage> message)
        {
            // I'm not sure what is the right behavior if a couple of
            // clients subscribe...
            // Maybe we should be able to map a message somehow to 
            // the client that requested it? This makes sense only in call
            // messages which have an Id..
            mClient = client;

            mConnection.OnNext(message);
        }
    }
}