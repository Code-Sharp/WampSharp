using WampSharp.V2.Client;

namespace WampSharp.V2.Fluent
{
    public static class ChannelFactoryExtensions
    {
        /// <summary>
        /// Indicates that the user wants to connect to a given realm.
        /// </summary>
        /// <param name="factory">The WAMP channel factory to use.</param>
        /// <param name="realm">The requested realm to connect to.</param>
        public static ChannelFactorySyntax.IRealmSyntax ConnectToRealm(this IWampChannelFactory factory, string realm)
        {
            return new ChannelState()
            {
                ChannelFactory = factory,
                Realm = realm
            };
        }

        /// <summary>
        /// Indicates that the user wants to use a given <see cref="IWampClientAuthenticator"/>.
        /// </summary>
        /// <param name="serializationSyntax">The given fluent syntax state.</param>
        /// <param name="authenticator">The given <see cref="IWampClientAuthenticator"/>.</param>
        public static ChannelFactorySyntax.IAuthenticationSyntax Authenticator(this ChannelFactorySyntax.ISerializationSyntax serializationSyntax,
            IWampClientAuthenticator authenticator)
        {
            ChannelState state = serializationSyntax.State;

            state.Authenticator = authenticator;

            return state;
        }
    }
}