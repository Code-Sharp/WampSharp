using System;

namespace WampSharp.V2.Fluent
{
    public static class WebSocketChannelFactoryExtensions
    {
        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(
            this ChannelFactorySyntax.IRealmSyntax realmSyntax, Uri address)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new WebSocketActivator(address);

            return state;
        }
    }
}