using WampSharp.Core.Contracts;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// A default implementation of <see cref="IWampClientAuthenticator"/>.
    /// </summary>
    public class DefaultWampClientAuthenticator : IWampClientAuthenticator
    {
        /// <summary>
        /// Just throws exception on CHALLENGE
        /// </summary>
        /// <param name="authmethod"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public AuthenticationResponse Authenticate(string authmethod, ChallengeDetails extra)
        {
            throw new WampAuthenticationException("Authorization was requested but no authenticator was provided");
        }

        public string[] AuthenticationMethods
        {
            get { return null; }
        }

        public string AuthenticationId
        {
            get { return null; }
        }
    }
}