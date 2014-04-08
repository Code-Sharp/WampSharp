using System;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : IWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;
        protected readonly object mLock = new object();
        protected bool mClosed = false;

        public FleckWampConnection(IWebSocketConnection webSocketConnection)
        {
            mWebSocketConnection = webSocketConnection;
            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
        }

        private void OnConnectionOpen()
        {
            RaiseConnectionOpen();
        }

        private void OnConnectionError(Exception exception)
        {
            RaiseConnectionError(exception);
        }

        private void OnConnectionClose()
        {
            lock (mLock)
            {
                if (!mClosed)
                {
                    mClosed = true;
                }
            }

            RaiseConnectionClosed();
        }

        private void RaiseConnectionOpen()
        {
            EventHandler connectionOpen = this.ConnectionOpen;

            if (connectionOpen != null)
            {
                connectionOpen(this, EventArgs.Empty);
            }
        }

        private void RaiseConnectionClosed()
        {
            EventHandler connectionClosed = this.ConnectionClosed;

            if (connectionClosed != null)
            {
                connectionClosed(this, EventArgs.Empty);
            }
        }

        protected void RaiseNewMessageArrived(WampMessage<TMessage> parsed)
        {
            var messageArrived = this.MessageArrived;

            if (messageArrived != null)
            {
                messageArrived(this, new WampMessageArrivedEventArgs<TMessage>(parsed));
            }
        }

        public void Dispose()
        {
            lock (mLock)
            {
                if (!mClosed)
                {
                    mWebSocketConnection.Close();
                }
            }
        }

        public void Send(WampMessage<TMessage> message)
        {
            lock (mLock)
            {
                if (!mClosed)
                {
                    InnerSend(message);
                }
            }
        }

        protected abstract void InnerSend(WampMessage<TMessage> message);

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        protected virtual void RaiseConnectionError(Exception ex)
        {
            EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;
            
            if (handler != null)
            {
                handler(this, new WampConnectionErrorEventArgs(ex));
            }
        }
    }
}