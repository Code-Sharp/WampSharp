using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledTextWebSocketConnection<TMessage> : TextWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledTextWebSocketConnection(Uri addressUri, IWampTextBinding<TMessage> binding) : this(new ClientWebSocket(), addressUri, binding)
        {
        }

        public ControlledTextWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri, IWampTextBinding<TMessage> binding) : base(clientWebSocket, addressUri, binding)
        {
        }

        public void Connect()
        {
            base.Connect();
        }
    }
}