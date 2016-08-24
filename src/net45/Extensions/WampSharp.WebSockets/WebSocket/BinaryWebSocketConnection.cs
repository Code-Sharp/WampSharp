using System;
using System.Net.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class BinaryWebSocketConnection<TMessage> : WebSocketConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public BinaryWebSocketConnection(WebSocket webSocket, IWampBinaryBinding<TMessage> binding, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) : 
            base(webSocket, binding, cookieProvider, cookieAuthenticatorFactory)
        {
            mBinding = binding;
        }

        protected BinaryWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri, IWampBinaryBinding<TMessage> binding) :
            base(clientWebSocket, addressUri, binding.Name, binding)
        {
            mBinding = binding;
        }

        protected override ArraySegment<byte> GetMessageInBytes(WampMessage<object> message)
        {
            byte[] bytes = mBinding.Format(message);
            return new ArraySegment<byte>(bytes);
        }

        protected override WebSocketMessageType WebSocketMessageType
        {
            get
            {
                return WebSocketMessageType.Binary;
            }
        }
    }
}