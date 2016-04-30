using System;
using System.Net.WebSockets;
using System.Text;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using Websocket = System.Net.WebSockets.WebSocket;

namespace WampSharp.WebSocket
{
    public class TextWebSocketConnection<TMessage> : WebSocketConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextWebSocketConnection(Websocket webSocket, IWampTextBinding<TMessage> binding) : 
            base(webSocket)
        {
            mBinding = binding;
        }

        protected TextWebSocketConnection(Uri addressUri, IWampTextBinding<TMessage> binding) :
            base(addressUri, binding.Name, binding)
        {
            mBinding = binding;
        }

        protected override ArraySegment<byte> GetMessageInBytes(WampMessage<object> message)
        {
            string formatted = mBinding.Format(message);

            byte[] bytes = Encoding.UTF8.GetBytes(formatted);

            return new ArraySegment<byte>(bytes);
        }

        protected override WebSocketMessageType WebSocketMessageType
        {
            get
            {
                return WebSocketMessageType.Text;
            }
        }
    }
}