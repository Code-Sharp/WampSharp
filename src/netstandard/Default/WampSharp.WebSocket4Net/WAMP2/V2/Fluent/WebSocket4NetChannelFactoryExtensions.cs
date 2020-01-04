using System;

namespace WampSharp.V2.Fluent
{
    public static class WebSocket4NetChannelFactoryExtensions
    {
        /// <summary>
        /// Indicates the user wants to use WebSocket transport and to
        /// connect to a given address.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="address">The server's address.</param>
        [Obsolete("Use WebSocket4NetTransport instead", false)]
        public static IWebSocket4NetTransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address)
        {
            return WebSocket4NetTransport(realmSyntax, address);
        }

        /// <summary>
        /// Indicates that the user want to use WebSocket transport, using a custom
        /// WebSocket4Net factory.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="factory">The custom <see cref="WebSocket4NetFactory"/> to use to create the WebSocket.</param>
        [Obsolete("Use WebSocket4NetTransport instead", false)]
        public static IWebSocket4NetTransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocket4NetFactory factory)
        {
            return WebSocket4NetTransport(realmSyntax, factory);
        }

        /// <summary>
        /// Indicates the user wants to use WebSocket transport and to
        /// connect to a given address.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="address">The server's address.</param>
        public static IWebSocket4NetTransportSyntax WebSocket4NetTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address)
        {
            WebSocket4NetActivator activator = new WebSocket4NetActivator(address);

            return GetWebSocketSyntax(realmSyntax, activator);
        }

        /// <summary>
        /// Indicates that the user want to use WebSocket transport, using a custom
        /// WebSocket4Net factory.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="factory">The custom <see cref="WebSocket4NetFactory"/> to use to create the WebSocket.</param>
        public static IWebSocket4NetTransportSyntax WebSocket4NetTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocket4NetFactory factory)
        {
            WebSocket4NetActivator activator = new WebSocket4NetActivator(factory);

            return GetWebSocketSyntax(realmSyntax, activator);
        }

        private static IWebSocket4NetTransportSyntax GetWebSocketSyntax(ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocket4NetActivator activator)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = activator;

            WebSocket4NetTransportSyntax syntax = new WebSocket4NetTransportSyntax(state);

            return syntax;
        }
    }
}