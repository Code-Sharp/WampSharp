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

        public IWebSocketTransportSyntax SetClientWebSocketOptions(Action<ClientWebSocketOptions> configureClientWebSocketOptions)
        {
            ((WebSocketActivator) State.ConnectionActivator).ConfigureOptions = configureClientWebSocketOptions;
            return this;
        }

        public IWebSocketTransportSyntax SetMaxFrameSize(int maxFrameSize)
        {
            ((WebSocketActivator) State.ConnectionActivator).MaxFrameSize = maxFrameSize;
            return this;
        }

        public ChannelState State { get; }
    }
}