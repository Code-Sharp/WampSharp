namespace WampSharp.V2.Fluent
{
    public static class WebSocket4NetChannelFactoryExtensions
    {
        /// <summary>
        /// Indicates the user wants to use WebSocket transport and to
        /// connect to a given address.
        /// </summary>
        /// <param name="address">The server's address.</param>
        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new WebSocket4NetActivator(address);

            return state;
        }

        /// <summary>
        /// Indicates that the user want to use WebSocket transport, using a custom
        /// WebSocket4Net factory.
        /// </summary>
        /// <param name="factory">The custom <see cref="WebSocket4NetFactory"/> to use to create the WebSocket.</param>
        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, WebSocket4NetFactory factory)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new WebSocket4NetActivator(factory);

            return state;
        }
    }
}