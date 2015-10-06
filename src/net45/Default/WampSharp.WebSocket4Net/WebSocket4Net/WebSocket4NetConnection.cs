using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public abstract class WebSocket4NetConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        #region Fields

        private readonly IWampBinding<TMessage> mBinding;

        private readonly WebSocket mWebSocket;
        
        #endregion

        public WebSocket4NetConnection(WebSocket webSocket,
                                       IWampBinding<TMessage> binding)
        {
            mBinding = binding;
            mWebSocket = webSocket;
            mWebSocket.Opened += WebSocketOnOpened;
            mWebSocket.Closed += WebSocketOnClosed;
            mWebSocket.Error += WebSocketOnError;
        }

        public WebSocket4NetConnection(string serverAddress,
                                       IWampBinding<TMessage> binding)
            : this(new WebSocket(serverAddress, binding.Name, WebSocketVersion.None), binding)
        {
        }

        public IWampBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }

        protected WebSocket WebSocket
        {
            get
            {
                return mWebSocket;
            }
        }


        private void WebSocketOnOpened(object sender, EventArgs eventArgs)
        {
            RaiseConnectionOpen();
        }

        private void WebSocketOnClosed(object sender, EventArgs eventArgs)
        {
            RaiseConnectionClosed();
        }

        private void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            RaiseConnectionError(e.Exception);
        }

        public void Connect()
        {
            mWebSocket.Open();
        }

        public virtual void Dispose()
        {
            mWebSocket.Close();
        }

        void IWampConnection<TMessage>.Send(WampMessage<object> message)
        {
            Send(message);
        }

        public abstract void Send(WampMessage<object> message);

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

        protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            EventHandler<WampMessageArrivedEventArgs<TMessage>> handler = MessageArrived;
            
            if (handler != null)
            {
                WampMessageArrivedEventArgs<TMessage> eventArgs = new WampMessageArrivedEventArgs<TMessage>(message);
                handler(this, eventArgs);
            }
        }
        
        protected virtual void RaiseConnectionOpen()
        {
            EventHandler handler = ConnectionOpen;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void RaiseConnectionClosed()
        {
            EventHandler handler = ConnectionClosed;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}