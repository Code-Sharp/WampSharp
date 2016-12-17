using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledBinaryWebSocketWrapperConnection<TMessage> : BinaryWebSocketWrapperConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledBinaryWebSocketWrapperConnection(Uri addressUri, IWampBinaryBinding<TMessage> binding) : this(new ClientWebSocket(), addressUri, binding)
        {
        }

        public ControlledBinaryWebSocketWrapperConnection(ClientWebSocket clientWebSocket, Uri addressUri, IWampBinaryBinding<TMessage> binding) : 
            base(new ClientWebSocketWrapper(clientWebSocket), addressUri, binding)
        {
        }

        public void Connect()
        {
            base.Connect();
        }
    }
}