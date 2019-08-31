using System;
using System.Net.WebSockets;

namespace WampSharp.V2.Fluent
{
    internal class WebSocketTransportSyntax : IWebSocketTransportSyntax
    {
        public WebSocketTransportSyntax(ChannelState state)
        {
            State = state;
        }

        public ChannelFactorySyntax.ITransportSyntax SetClientWebSocketOptions(Action<ClientWebSocketOptions> configureClientWebSocketOptions)
        {
            ((WebSocketActivator) State.ConnectionActivator).ConfigureOptions = configureClientWebSocketOptions;
            return this;
        }

        public ChannelState State { get; }
    }
}