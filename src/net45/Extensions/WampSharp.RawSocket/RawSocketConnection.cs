using System;
using System.Threading;
using SuperSocket.SocketBase;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    internal class RawSocketConnection<TMessage> : IWampConnection<TMessage>
    {
        private readonly RawSocketSession mConnection;
        private readonly RawSocketBinding<TMessage> mBinding;

        public RawSocketConnection(RawSocketSession connection, IWampTextBinding<TMessage> binding)
            : this(connection, new RawSocketBinding<TMessage>(binding))
        {
        }

        public RawSocketConnection(RawSocketSession connection, IWampBinaryBinding<TMessage> binding)
            : this(connection, new RawSocketBinding<TMessage>(binding))
        {
        }
        
        private RawSocketConnection(RawSocketSession connection, RawSocketBinding<TMessage> binding)
        {
            mConnection = connection;
            mConnection.SessionStarted += OnSessionStarted;
            mConnection.MessageArrived += OnMessageArrived;
            mConnection.SessionClosed += OnSessionClosed;
            mBinding = binding;
        }

        private void OnSessionStarted(object sender, EventArgs e)
        {
            RaiseConnectionOpen();
        }

        private void OnSessionClosed(object sender, CloseReason e)
        {
            RaiseConnectionClosed();
        }

        private void OnMessageArrived(object sender, MessageArrivedEventArgs e)
        {
            WampMessage<TMessage> parsed = mBinding.Parse(e.Message);
            RaiseMessageArrived(new WampMessageArrivedEventArgs<TMessage>(parsed));
        }

        public void Dispose()
        {
            mConnection.SessionStarted -= OnSessionStarted;
            mConnection.SessionClosed -= OnSessionClosed;
            mConnection.MessageArrived -= OnMessageArrived;
            mConnection.Close();
        }

        public void Send(WampMessage<object> message)
        {
            byte[] bytes = mBinding.Format(message);

            mConnection.Send(bytes, 0, bytes.Length);
        }

        #region Events

        private void RaiseConnectionOpen()
        {
            EventHandler handler = ConnectionOpen;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RaiseConnectionClosed()
        {
            var handler = ConnectionClosed;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> wampMessageArrivedEventArgs)
        {
            EventHandler<WampMessageArrivedEventArgs<TMessage>> onMessageArrived = this.MessageArrived;

            if (onMessageArrived != null)
            {
                onMessageArrived(this, wampMessageArrivedEventArgs);
            }
        }

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        #endregion
    }
}