using System;
using System.Net.WebSockets;

namespace WampSharp.V2.Fluent
{
    internal class WebSocketTransportSyntax : IWebSocketTransportSyntax
    {
        private readonly ChannelState mState;

        public WebSocketTransportSyntax(ChannelState state)
        {
            mState = state;
        }

        public ChannelFactorySyntax.ITransportSyntax SetClientWebSocketOptions(Action<ClientWebSocketOptions> configureClientWebSocketOptions)
        {
            ((WebSocketActivator) State.ConnectionActivator).ConfigureOptions = configureClientWebSocketOptions;
            return this;
        }

        public ChannelState State
        {
            get
            {
                return mState;
            }
        }
    }
}