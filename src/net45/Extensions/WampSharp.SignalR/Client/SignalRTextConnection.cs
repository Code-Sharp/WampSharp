using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.SignalR
{
    /// <summary>
    /// Represents a <see cref="IWampConnection{TMessage}"/> implemented using SignalR.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class SignalRTextConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;
        private readonly Connection mConnection;
        private readonly IClientTransport mTransport;

        /// <summary>
        /// Creates a new instance of <see cref="SignalRTextConnection{TMessage}"/>.
        /// </summary>
        /// <param name="uri">The uri of the server to connect to.</param>
        /// <param name="binding">The binding to use.</param>
        /// <param name="transport">The <see cref="IClientTransport"/> to use.</param>
        public SignalRTextConnection(string uri, IWampTextBinding<TMessage> binding, IClientTransport transport)
        {
            mBinding = binding;
            mTransport = transport;
            mConnection = new Connection(uri);

            mConnection.Closed += OnClosed;
            mConnection.Error += OnError;
            mConnection.Received += OnReceived;
        }

        private void OnReceived(string text)
        {
            WampMessage<TMessage> message = mBinding.Parse(text);
            this.RaiseMessageArrived(new WampMessageArrivedEventArgs<TMessage>(message));
        }

        private void OnError(Exception exception)
        {
            this.RaiseConnectionError(new WampConnectionErrorEventArgs(exception));
        }

        private void OnClosed()
        {
            this.RaiseConnectionClosed();
        }

        public void Connect()
        {
            mConnection.Start(mTransport)
                .ContinueWith(x =>
                                  {
                                      x.Wait();
                                      RaiseConnectionOpen();
                                  });
        }

        public void Dispose()
        {
            mConnection.Dispose();
        }

        public void Send(WampMessage<TMessage> message)
        {
            string text = mBinding.Format(message);
            mConnection.Send(text);
        }

        protected virtual void RaiseConnectionOpen()
        {
            EventHandler handler = ConnectionOpen;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> e)
        {
            EventHandler<WampMessageArrivedEventArgs<TMessage>> handler = MessageArrived;
            if (handler != null) handler(this, e);
        }

        protected virtual void RaiseConnectionClosed()
        {
            EventHandler handler = ConnectionClosed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionError(WampConnectionErrorEventArgs e)
        {
            EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Occurs when this connection opens.
        /// </summary>
        public event EventHandler ConnectionOpen;

        /// <summary>
        /// Occurs when a message arrives.
        /// </summary>
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;

        /// <summary>
        /// Occurs when this connection closes.
        /// </summary>
        public event EventHandler ConnectionClosed;

        /// <summary>
        /// Occurs when this connection occurs an error.
        /// </summary>
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}