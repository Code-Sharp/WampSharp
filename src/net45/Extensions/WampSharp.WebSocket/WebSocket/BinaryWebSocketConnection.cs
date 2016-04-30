using System;
using System.IO;
using System.Net.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using Websocket = System.Net.WebSockets.WebSocket;

namespace WampSharp.WebSocket
{
    public class BinaryWebSocketConnection<TMessage> : WebSocketConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public BinaryWebSocketConnection(Websocket webSocket, IWampBinaryBinding<TMessage> binding) : 
            base(webSocket)
        {
            mBinding = binding;
        }

        protected BinaryWebSocketConnection(Uri addressUri, IWampBinaryBinding<TMessage> binding) :
            base(addressUri, binding.Name, binding)
        {
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