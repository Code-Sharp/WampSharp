using WampSharp.V2.Client;

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

        public static ChannelFactorySyntax.IAuthenticationSyntax Authenticator(this ChannelFactorySyntax.ISerializationSyntax serializationSyntax,
            IWampClientAuthenticator authenticator)
        {
            ChannelState state = serializationSyntax.State;

            state.Authenticator = authenticator;

            return state;
        }
    }
}