using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Logging;
using WampSharp.V2.Binding;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public abstract class WebSocket4NetConnection<TMessage> : IControlledWampConnection<TMessage>
    {

        #region Fields

        private readonly WebSocket mWebSocket;

        private bool mDisposed = false;

        private readonly ILog mLogger;

        #endregion

        public WebSocket4NetConnection(WebSocket webSocket,
                                       IWampBinding<TMessage> binding)
        {
            Binding = binding;
            mWebSocket = webSocket;
            mLogger = LogProvider.GetLogger(this.GetType());
            mWebSocket.Opened += WebSocketOnOpened;
            mWebSocket.Closed += WebSocketOnClosed;
            mWebSocket.Error += WebSocketOnError;
        }

        public WebSocket4NetConnection(string serverAddress,
                                       IWampBinding<TMessage> binding)
            : this(serverAddress: serverAddress, binding: binding, configureSecurityOptions: null)
        {
        }

        public WebSocket4NetConnection(string serverAddress,
                                       IWampBinding<TMessage> binding, 
                                       Action<SecurityOption> configureSecurityOptions)
            : this(new WebSocket(serverAddress, binding.Name, WebSocketVersion.None), binding)
        {
            configureSecurityOptions?.Invoke(WebSocket.Security);
        }

        public IWampBinding<TMessage> Binding { get; }

        protected WebSocket WebSocket => mWebSocket;


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
            mLogger.Error("A connection error occurred", e.Exception);
            RaiseConnectionError(e.Exception);
        }

        public void Connect()
        {
            mWebSocket.Open();
        }

        public virtual void Dispose()
        {
            if (!mDisposed)
            {
                mDisposed = true;
                mWebSocket.Close();
            }
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
            ConnectionError?.Invoke(this, new WampConnectionErrorEventArgs(ex));
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
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}