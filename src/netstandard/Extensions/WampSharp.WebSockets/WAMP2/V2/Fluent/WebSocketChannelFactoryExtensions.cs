using System;
using WampSharp.WebSockets;

namespace WampSharp.V2.Fluent
{
    public static class WebSocketChannelFactoryExtensions
    {
        public static IWebSocketTransportSyntax WebSocketTransport(
            this ChannelFactorySyntax.IRealmSyntax realmSyntax, Uri address)
        {
            IWampConnectionActivator connectionActivator = new WebSocketActivator(address);

            return GetWebSocketTransportSyntax(realmSyntax, connectionActivator);
        }

        public static IWebSocketTransportSyntax WebSocketTransport(
            this ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocketFactory webSocketFactory, Uri address)
        {
            WebSocketActivator connectionActivator = new WebSocketActivator(address) {WebSocketFactory = webSocketFactory};

            return GetWebSocketTransportSyntax(realmSyntax, connectionActivator);
        }

        private static IWebSocketTransportSyntax GetWebSocketTransportSyntax
        (ChannelFactorySyntax.IRealmSyntax realmSyntax,
         IWampConnectionActivator connectionActivator)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = connectionActivator;

            WebSocketTransportSyntax result = new WebSocketTransportSyntax(state);

            return result;
        }
    }
}