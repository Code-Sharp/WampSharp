using System;
using WebSocket4Net;

namespace WampSharp.V2.Fluent
{
    public static class WebSocket4NetChannelFactoryExtensions
    {
        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new WebSocket4NetActivator(address);

            return state;
        }

        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocket4NetFactory factory)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new WebSocket4NetActivator(factory);

            return state;
        }
    }
}