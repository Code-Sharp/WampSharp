using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledTextWebSocketWrapperConnection<TMessage> : TextWebSocketWrapperConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledTextWebSocketWrapperConnection(Uri addressUri, IWampTextBinding<TMessage> binding) : this(new ClientWebSocket(), addressUri, binding)
        {
        }

        public ControlledTextWebSocketWrapperConnection(ClientWebSocket clientWebSocket, Uri addressUri, IWampTextBinding<TMessage> binding) : base(new ClientWebSocketWrapper(clientWebSocket), addressUri, binding)
        {
        }

        public void Connect()
        {
            base.Connect();
        }
    }
}