using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    public class ReviveClientConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        private readonly Func<IControlledWampConnection<TMessage>> mFactory;
        private IControlledWampConnection<TMessage> mConnection;

        public ReviveClientConnection(Func<IControlledWampConnection<TMessage>> factory)
        {
            mFactory = factory;
        }

        public void Connect()
        {
            if (mConnection == null)
            {
                mConnection = SetupConnection();
            }

            mConnection.Connect();
        }

        private IControlledWampConnection<TMessage> SetupConnection()
        {
            IControlledWampConnection<TMessage> result = mFactory();

            result.ConnectionOpen += OnConnectionOpen;
            result.ConnectionClosed += OnConnectionClosed;
            result.ConnectionError += OnConnectionError;
            result.MessageArrived += OnMessageArrived;

            return result;
        }

        private void OnMessageArrived(object sender, WampMessageArrivedEventArgs<TMessage> e)
        {
            RaiseMessageArrived(e);
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            RaiseConnectionError(e);
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            DestroyConnection();

            RaiseConnectionClosed();
        }

        private void DestroyConnection()
        {
            mConnection.ConnectionOpen -= OnConnectionOpen;
            mConnection.ConnectionClosed -= OnConnectionClosed;
            mConnection.ConnectionError -= OnConnectionError;
            mConnection.MessageArrived -= OnMessageArrived;

            mConnection.Dispose();
            mConnection = null;
        }

        private void OnConnectionOpen(object sender, EventArgs e)
        {
            RaiseConnectionOpen();
        }

        public void Dispose()
        {
            IControlledWampConnection<TMessage> connection = mConnection;

            if (connection != null)
            {
                connection.Dispose();
            }
        }

        public void Send(WampMessage<object> message)
        {
            IControlledWampConnection<TMessage> connection = mConnection;

            if (connection != null)
            {
                connection.Send(message);
            }
        }

        public event EventHandler ConnectionOpen;

        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;

        public event EventHandler ConnectionClosed;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        protected virtual void RaiseConnectionOpen()
        {
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> e)
        {
            MessageArrived?.Invoke(this, e);
        }

        protected virtual void RaiseConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionError(WampConnectionErrorEventArgs e)
        {
            ConnectionError?.Invoke(this, e);
        }
    }
}