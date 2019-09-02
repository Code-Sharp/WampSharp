using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.Logging;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Listens to <see cref="IWampConnection{TMessage}"/>s, receives 
    /// <see cref="WampMessage{TMessage}"/>s and dispatches them to a given <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampListener<TMessage, TClient>
    {
        private readonly IWampIncomingMessageHandler<TMessage, TClient> mHandler;
        private readonly IWampConnectionListener<TMessage> mListener;
        private IDisposable mSubscription;
        protected readonly ILog mLogger;

        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage, TClient}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        /// accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/> used
        /// in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        /// in order to store the connected clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, TClient> handler,
                            IWampClientContainer<TMessage, TClient> clientContainer)
        {
            mHandler = handler;
            ClientContainer = clientContainer;
            mListener = listener;
            mLogger = LogProvider.GetLogger(this.GetType());
        }

        /// <summary>
        /// The <see cref="IWampClientContainer{TMessage,TClient}"/>
        /// holding all current connected clients.
        /// </summary>
        public IWampClientContainer<TMessage, TClient> ClientContainer { get; }

        /// <summary>
        /// Starts listening for <see cref="IWampConnection{TMessage}"/>s.
        /// </summary>
        public virtual void Start()
        {
            mSubscription = mListener.Subscribe(x => OnNewConnection(x));
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
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
            OnCloseConnection(connection);
        }

        protected virtual void OnCloseConnection(IWampConnection<TMessage> connection)
        {
            ClientContainer.RemoveClient(connection);


            // Prefer the non-blocking version
            if (connection is IAsyncDisposable asyncDisposable)
            {
                asyncDisposable.DisposeAsync();
            }
            else
            {
                connection.Dispose();
            }
        }

        protected virtual void OnNewMessage(IWampConnection<TMessage> connection, WampMessage<TMessage> message)
        {
            TClient client = ClientContainer.GetClient(connection);

            using (IDisposable sessionIdMappedContext = SessionIdMappedContext(client))
            {
                mHandler.HandleMessage(client, message);
            }
        }

        private IDisposable SessionIdMappedContext(TClient client)
        {
            object sessionIdValue = GetSessionId(client);
            string sessionIdString = null;

            if (sessionIdValue != null)
            {
                sessionIdString = sessionIdValue.ToString();
            }

            IDisposable disposable =
                LogProvider.OpenMappedContext("WampSessionId", sessionIdString);

            return disposable;
        }

        protected virtual object GetSessionId(TClient client)
        {
            return null;
        }

        protected virtual void OnNewConnection(IWampConnection<TMessage> connection)
        {
            TClient client = ClientContainer.GetClient(connection);

            connection.MessageArrived += OnNewMessage;
            connection.ConnectionOpen += OnConnectionOpen;
            connection.ConnectionError += OnConnectionError;
            connection.ConnectionClosed += OnConnectionClose;
        }

        protected virtual void OnConnectionOpen(IWampConnection<TMessage> connection)
        {
        }

        private void OnNewMessage(object sender, WampMessageArrivedEventArgs<TMessage> e)
        {
            IWampConnection<TMessage> connection = sender as IWampConnection<TMessage>;
            OnNewMessage(connection, e.Message);
        }

        private void OnConnectionOpen(object sender, EventArgs e)
        {
            IWampConnection<TMessage> connection = sender as IWampConnection<TMessage>;
            connection.ConnectionOpen -= OnConnectionOpen;
            OnConnectionOpen(connection);
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            OnConnectionClosed(sender);
        }

        private void OnConnectionClose(object sender, EventArgs e)
        {
            OnConnectionClosed(sender);
        }

        private void OnConnectionClosed(object sender)
        {
            IWampConnection<TMessage> connection = sender as IWampConnection<TMessage>;
            connection.ConnectionClosed -= OnConnectionClose;
            connection.MessageArrived -= OnNewMessage;
            connection.ConnectionError -= OnConnectionError;
            OnCloseConnection(connection);
        }
    }
}