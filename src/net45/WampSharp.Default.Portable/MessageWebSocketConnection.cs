using System;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.Default
{
    public abstract class MessageWebSocketConnection<TMessage> : AsyncWampConnection<TMessage>,
        IControlledWampConnection<TMessage>
    {
        private readonly MessageWebSocket mWebSocket;
        private readonly string mUri;
        private readonly IWampBinding<TMessage> mBinding;
        private bool mIsConnected;

        public MessageWebSocketConnection(MessageWebSocket webSocket,
            string uri,
            IWampBinding<TMessage> binding, SocketMessageType messageType)
        {
            mWebSocket = webSocket;
            mWebSocket.Control.MessageType = messageType;
            mWebSocket.Control.SupportedProtocols.Add(binding.Name);
            mUri = uri;
            mBinding = binding;
            mWebSocket.MessageReceived += OnMessageReceived;
            mWebSocket.Closed += OnClosed;
        }

        protected abstract void OnMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args);

        protected void OnClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            mIsConnected = false;
            RaiseConnectionClosed();
        }

        protected override bool IsConnected
        {
            get
            {
                return mIsConnected;
            }
        }

        protected abstract override Task SendAsync(WampMessage<object> message);

        public async void Connect()
        {
            try
            {
                await mWebSocket.ConnectAsync(new Uri(mUri));
                mIsConnected = true;
                RaiseConnectionOpen();
            }
            catch (Exception ex)
            {                
                RaiseConnectionError(ex);
                RaiseConnectionClosed();
            }
        }

        public override void Dispose()
        {
            mWebSocket.Dispose();
        }
    }
}