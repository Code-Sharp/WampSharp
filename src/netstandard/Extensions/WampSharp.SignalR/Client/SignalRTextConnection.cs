using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class SignalRTextConnection<TMessage> :
        AsyncWampConnection<TMessage>,
        IControlledWampConnection<TMessage>
    {
        private int mIsConnected = 0;
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
            this.RaiseMessageArrived(message);
        }

        private void OnError(Exception exception)
        {
            this.RaiseConnectionError(exception);
        }

        private void OnClosed()
        {
            this.RaiseConnectionClosed();
        }

        public async void Connect()
        {
            await mConnection.Start(mTransport).ConfigureAwait(false);
            RaiseConnectionOpen();
        }

        protected override void RaiseConnectionOpen()
        {
            Interlocked.CompareExchange(ref mIsConnected, 1, 0);

            base.RaiseConnectionOpen();
        }

        protected override void RaiseConnectionClosed()
        {
            Interlocked.CompareExchange(ref mIsConnected, 0, 1);

            base.RaiseConnectionClosed();
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            string text = mBinding.Format(message);
            return mConnection.Send(text);
        }

        protected override void Dispose()
        {
            mConnection.Dispose();
        }

        protected override bool IsConnected => mIsConnected == 1;
    }
}