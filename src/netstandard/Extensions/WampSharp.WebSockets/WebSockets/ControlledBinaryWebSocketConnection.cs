using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledBinaryWebSocketConnection<TMessage> : BinaryWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledBinaryWebSocketConnection(Uri addressUri, IWampBinaryBinding<TMessage> binding,
                                                   int? maxFrameSize) : 
            this(new ClientWebSocket(), addressUri, binding, maxFrameSize)
        {
        }

        public ControlledBinaryWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri,
                                                   IWampBinaryBinding<TMessage> binding, int? maxFrameSize) :
            base(clientWebSocket, addressUri, binding, maxFrameSize)
        {
        }

        public void Connect()
        {
            base.InnerConnect();
        }
    }
}