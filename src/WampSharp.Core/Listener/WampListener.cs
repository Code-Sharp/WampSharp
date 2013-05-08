using System;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    public class WampListener<TMessage>
    {
        private readonly IWampIncomingMessageHandler<TMessage> mHandler;
        private readonly IWampClientContainer<TMessage> mClientContainer;
        private readonly IWampConnectionListener<TMessage> mListener;

        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage> handler,
                            IWampClientContainer<TMessage> clientContainer)
        {
            mHandler = handler;
            mClientContainer = clientContainer;
            mListener = listener;
        }

        public void Start()
        {
            mListener.Subscribe(x => OnNewConnection(x));
        }

        private void OnConnectionException(IWampConnection<TMessage> connection, Exception exception)
        {
        }

        private void OnCloseConnection(IWampConnection<TMessage> connection)
        {
            IDisposable client = mClientContainer.GetClient(connection) as IDisposable;

            if (client != null)
            {
                client.Dispose();
            }
        }

        private void OnNewMessage(IWampConnection<TMessage> connection, WampMessage<TMessage> message)
        {
            IWampClient client = mClientContainer.GetClient(connection);

            mHandler.HandleMessage(client, message);
        }

        private void OnNewConnection(IWampConnection<TMessage> connection)
        {
            IWampClient client = mClientContainer.GetClient(connection);

            connection.Subscribe
                (x => OnNewMessage(connection, x),
                 ex => OnConnectionException(connection, ex),
                 () => OnCloseConnection(connection));

            client.Welcome(client.SessionId, 1, "WampSharp");
        }
    }
}