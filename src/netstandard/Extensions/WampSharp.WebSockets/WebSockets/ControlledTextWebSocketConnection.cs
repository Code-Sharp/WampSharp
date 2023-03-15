using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledTextWebSocketConnection<TMessage> : TextWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledTextWebSocketConnection(Uri addressUri, IWampTextBinding<TMessage> binding, int? maxFrameSize) : 
            this(new ClientWebSocket(), addressUri, binding, maxFrameSize)
        {
        }

        public ControlledTextWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri,
                                                 IWampTextBinding<TMessage> binding, int? maxFrameSize) : base(clientWebSocket, addressUri, binding, maxFrameSize)
        {
        }

        public void Connect()
        {
            base.InnerConnect();
        }
    }
}