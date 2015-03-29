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
        protected MessageWebSocket mWebSocket;
        private readonly string mUri;
        private readonly IWampBinding<TMessage> mBinding;
        private bool mIsConnected;
        private readonly SocketMessageType mSocketMessageType;

        protected MessageWebSocketConnection(string uri,
            IWampBinding<TMessage> binding, SocketMessageType messageType)
        {
            mUri = uri;
            mBinding = binding;
            mSocketMessageType = messageType;
        }

        private MessageWebSocket CreateWebSocket()
        {
            MessageWebSocket socket = new MessageWebSocket();
            socket.Control.MessageType = mSocketMessageType;
            socket.Control.SupportedProtocols.Add(mBinding.Name);
            socket.MessageReceived += OnMessageReceived;
            socket.Closed += OnClosed;
            return socket;
        }

        protected abstract void OnMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args);

        protected void OnClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            mWebSocket = null;
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
                mWebSocket = CreateWebSocket();
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