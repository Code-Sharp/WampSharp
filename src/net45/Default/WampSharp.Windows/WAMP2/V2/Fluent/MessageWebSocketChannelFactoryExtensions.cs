namespace WampSharp.V2.Fluent
{
    public static class MessageWebSocketChannelFactoryExtensions
    {
        public static ChannelFactorySyntax.ITransportSyntax WebSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address)
        {
            ChannelState state = realmSyntax.State;

            state.ConnectionActivator = new MessageWebSocketActivator(address);

            return state;
        }
    }
}