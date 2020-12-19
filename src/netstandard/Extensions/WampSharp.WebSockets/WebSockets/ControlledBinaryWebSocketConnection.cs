using System;
using System.Net.WebSockets;
using System.Threading;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class ControlledBinaryWebSocketConnection<TMessage> : BinaryWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledBinaryWebSocketConnection(Uri addressUri, IWampBinaryBinding<TMessage> binding) : 
            this(new ClientWebSocket(), addressUri, binding)
        {
        }

        public ControlledBinaryWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri, IWampBinaryBinding<TMessage> binding) :
            base(clientWebSocket, addressUri, binding)
        {
        }

        public void Connect(CancellationToken cancellationToken)
        {
            base.InnerConnect(cancellationToken);
        }
    }
}