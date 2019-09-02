using WampSharp.Core.Cra;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// An authenticator that uses WAMP-CRA authentication method.
    /// </summary>
    public class WampCraClientAuthenticator : IWampClientAuthenticator
    {
        private const string WAMP_CRA = WampAuthenticationMethods.WampCra;
        private readonly string mAuthenticationKey;
        private readonly string mSecret;

        /// <summary>
        /// Initializes a new instance of a <see cref="WampCraClientAuthenticator"/>.
        /// </summary>
        /// <param name="authenticationId">The authentication id to use (for example, the user name)</param>
        /// <param name="secret">The secret to use.</param>
        /// <param name="salt">The salt to use.</param>
        /// <param name="iterations">The number of iterations to use (default value = 1000).</param>
        /// <param name="keyLen">The key length to use (default value = 32).</param>
        public WampCraClientAuthenticator(string authenticationId,
                                          string secret,
                                          string salt = null,
                                          int? iterations = null,
                                          int? keyLen = null)
        {
            mSecret = secret;
            AuthenticationId = authenticationId;
            mAuthenticationKey = WampCraHelpers.DeriveKey(secret, salt, iterations, keyLen);
        }

        public AuthenticationResponse Authenticate(string authmethod, ChallengeDetails extra)
        {
            if (authmethod == WAMP_CRA)
            {
                WampCraChallengeDetails challengeDetails = 
                    extra.OriginalValue.Deserialize<WampCraChallengeDetails>();
                
                string signature;

                if (challengeDetails.Salt == null)
                {
                    signature =
                        WampCraHelpers.Sign(mAuthenticationKey,
                                            challengeDetails.Challenge);
                }
                else
                {
                    signature =
                        WampCraHelpers.AuthSignature(challengeDetails.Challenge,
                                                     mSecret,
                                                     challengeDetails);
                }

                AuthenticationResponse result =
                    new AuthenticationResponse { Signature = signature };

                return result;
            }
            else
            {
                throw new WampAuthenticationException("don't know how to authenticate using '" + authmethod + "'");
            }
        }

        public string[] AuthenticationMethods => new string[] {WAMP_CRA};

        public string AuthenticationId { get; }
    }
}