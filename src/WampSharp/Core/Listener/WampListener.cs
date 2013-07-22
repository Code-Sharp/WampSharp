using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    public class WampListener<TMessage, TClient>
    {
        private readonly IWampIncomingMessageHandler<TMessage, TClient> mHandler;
        private readonly IWampClientContainer<TMessage, TClient> mClientContainer;
        private readonly IWampConnectionListener<TMessage> mListener;
        private IDisposable mSubscription;

        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, TClient> handler,
                            IWampClientContainer<TMessage, TClient> clientContainer)
        {
            mHandler = handler;
            mClientContainer = clientContainer;
            mListener = listener;
        }

        public IWampClientContainer<TMessage, TClient> ClientContainer
        {
            get
            {
                return mClientContainer;
            }
        }

        public virtual void Start()
        {
            mSubscription = mListener.Subscribe(x => OnNewConnection(x));
        }

        public virtual void Stop()
        {
            IDisposable subscription = mSubscription;

            if (subscription != null)
            {
                subscription.Dispose();
                mSubscription = null;
            }
        }

        protected virtual void OnConnectionException(IWampConnection<TMessage> connection, Exception exception)
        {
        }

        protected virtual void OnCloseConnection(IWampConnection<TMessage> connection)
        {
            IDisposable client = ClientContainer.GetClient(connection) as IDisposable;

            if (client != null)
            {
                client.Dispose();
            }
        }

        protected virtual void OnNewMessage(IWampConnection<TMessage> connection, WampMessage<TMessage> message)
        {
            TClient client = ClientContainer.GetClient(connection);

            mHandler.HandleMessage(client, message);
        }

        protected virtual void OnNewConnection(IWampConnection<TMessage> connection)
        {
            TClient client = ClientContainer.GetClient(connection);

            connection.Subscribe
                (x => OnNewMessage(connection, x),
                 ex => OnConnectionException(connection, ex),
                 () => OnCloseConnection(connection));
        }
    }
}