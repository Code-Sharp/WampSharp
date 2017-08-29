using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    /// <summary>
    /// WebSocket4NetConnection abstract class
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class WebSocket4NetConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        #region Fields

        private readonly IWampBinding<TMessage> mBinding;

        private readonly WebSocket mWebSocket;
        
        #endregion
        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="binding"></param>
        public WebSocket4NetConnection(WebSocket webSocket,
                                       IWampBinding<TMessage> binding)
        {
            mBinding = binding;
            mWebSocket = webSocket;
            mWebSocket.Opened += WebSocketOnOpened;
            mWebSocket.Closed += WebSocketOnClosed;
            mWebSocket.Error += WebSocketOnError;
        }
        /// <summary>
        /// Server address constructor
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="binding"></param>
        public WebSocket4NetConnection(string serverAddress,
                                       IWampBinding<TMessage> binding)
            : this(serverAddress: serverAddress, binding: binding, configureSecurityOptions: null)
        {
        }
        /// <summary>
        /// Server address constructor with <see cref="Action{SecurityOption}"/>
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="binding"></param>
        /// <param name="configureSecurityOptions"></param>
        public WebSocket4NetConnection(string serverAddress,
                                       IWampBinding<TMessage> binding, 
                                       Action<SecurityOption> configureSecurityOptions)
            : this(new WebSocket(serverAddress, binding.Name, WebSocketVersion.None), binding)
        {
            configureSecurityOptions?.Invoke(WebSocket.Security);
        }
        /// <summary>
        /// Wamp Binding object
        /// </summary>
        public IWampBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }
        /// <summary>
        /// WebSockect object
        /// </summary>
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
        /// <summary>
        /// Opens websocket connection
        /// </summary>
        public void Connect()
        {
            mWebSocket.Open();
        }
        /// <summary>
        ///  Disposes instance
        /// </summary>
        public virtual void Dispose()
        {
            mWebSocket.Close();
        }

        void IWampConnection<TMessage>.Send(WampMessage<object> message)
        {
            Send(message);
        }
        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="message"></param>
        public abstract void Send(WampMessage<object> message);

        /// <summary>
        /// Connection Open event
        /// </summary>
        public event EventHandler ConnectionOpen;
        /// <summary>
        /// Message Arrived event
        /// </summary>
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        /// <summary>
        /// Connection Closed event
        /// </summary>
        public event EventHandler ConnectionClosed;
        /// <summary>
        /// Connection Error event
        /// </summary>
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        /// <summary>
        /// Raises ConnectionError event
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void RaiseConnectionError(Exception ex)
        {
            ConnectionError?.Invoke(this, new WampConnectionErrorEventArgs(ex));
        }
        /// <summary>
        /// Raises MessageArrived event
        /// </summary>
        /// <param name="message"></param>
        protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            EventHandler<WampMessageArrivedEventArgs<TMessage>> handler = MessageArrived;
            
            if (handler != null)
            {
                WampMessageArrivedEventArgs<TMessage> eventArgs = new WampMessageArrivedEventArgs<TMessage>(message);
                handler(this, eventArgs);
            }
        }
        /// <summary>
        /// Raises ConnectionOpen Event
        /// </summary>
        protected virtual void RaiseConnectionOpen()
        {
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Disposes instance
        /// </summary>
        protected virtual void RaiseConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}