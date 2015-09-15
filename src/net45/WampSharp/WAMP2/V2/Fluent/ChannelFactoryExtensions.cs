namespace WampSharp.V2.Fluent
{
    public static class ChannelFactoryExtensions
    {
        public static ChannelFactorySyntax.IRealmSyntax ConnectToRealm(this IWampChannelFactory factory, string realm)
        {
            return new ChannelState()
            {
                ChannelFactory = factory,
                Realm = realm
            };
        }
    }
}