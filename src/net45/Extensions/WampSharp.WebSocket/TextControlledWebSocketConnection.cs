using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSocket
{
    public class TextControlledWebSocketConnection<TMessage> : TextWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        private readonly Uri mAddressUri;
        private readonly ClientWebSocket mClientWebSocket;

        public TextControlledWebSocketConnection(Uri addressUri, IWampTextBinding<TMessage> binding) :
            this(new ClientWebSocket(), binding)
        {
            mAddressUri = addressUri;
        }

        private TextControlledWebSocketConnection(ClientWebSocket webSocket, IWampTextBinding<TMessage> binding) : 
            base(webSocket, binding)
        {
            mClientWebSocket = webSocket;
            mClientWebSocket.Options.AddSubProtocol(binding.Name);
        }

        public async void Connect()
        {
            await mClientWebSocket.ConnectAsync(mAddressUri, mCancellationTokenSource.Token)
                .ConfigureAwait(false);

            RaiseConnectionOpen();

            Task task = Task.Run(this.RunAsync, mCancellationTokenSource.Token);
        }
    }
}