using System;
using System.IO;
using System.Net.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using Websocket = System.Net.WebSockets.WebSocket;

namespace WampSharp.WebSocket
{
    public class BinaryAspNetWebSocketConnection<TMessage> : WebSocketConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public BinaryAspNetWebSocketConnection(Websocket webSocket, IWampBinaryBinding<TMessage> binding) :
            base(webSocket)
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

        protected override void OnNewMessage(MemoryStream payloadData)
        {
            WampMessage<TMessage> message = mBinding.Parse(payloadData);
            RaiseMessageArrived(message);
        }
    }
}