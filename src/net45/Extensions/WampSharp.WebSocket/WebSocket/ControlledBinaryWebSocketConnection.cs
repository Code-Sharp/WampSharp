using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSocket
{
    public class ControlledBinaryWebSocketConnection<TMessage> : BinaryWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledBinaryWebSocketConnection(Uri addressUri, IWampBinaryBinding<TMessage> binding) : base(addressUri, binding)
        {
        }

        public void Connect()
        {
            base.Connect();
        }
    }
}