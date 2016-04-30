using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.WebSocket
{
    public class ControlledTextWebSocketConnection<TMessage> : TextWebSocketConnection<TMessage>, IControlledWampConnection<TMessage>
    {
        public ControlledTextWebSocketConnection(Uri addressUri, IWampTextBinding<TMessage> binding) : base(addressUri, binding)
        {
        }

        public void Connect()
        {
            base.Connect();
        }
    }
}