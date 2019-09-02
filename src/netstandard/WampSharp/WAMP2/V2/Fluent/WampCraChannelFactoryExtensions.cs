using WampSharp.V2.Client;

namespace WampSharp.V2.Fluent
{
    public static class WampCraChannelFactoryExtensions
    {
        public static ChannelFactorySyntax.IAuthenticationSyntax CraAuthentication(this ChannelFactorySyntax.ISerializationSyntax serializationSyntax, string authenticationId, string secret, string salt = null, int? iterations = null, int? keyLen = null)
        {
            ChannelState state = serializationSyntax.State;

            state.Authenticator = new WampCraClientAuthenticator(authenticationId, secret, salt, iterations, keyLen);

            return state;
        }
    }
}