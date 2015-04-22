using WampSharp.Core.Cra;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// An authenticator that uses WAMP-CRA authentication method.
    /// </summary>
    public class WampCraClientAuthenticator : IWampClientAuthenticator
    {
        private const string WAMP_CRA = "wampcra";
        private readonly string mAuthenticationId;
        private readonly string mAuthenticationKey;

        /// <summary>
        /// Initializes a new instance of a <see cref="WampCraClientAuthenticator"/>.
        /// </summary>
        /// <param name="authenticationId">The authentication id to use (for example, the user name)</param>
        /// <param name="authenticationKey">The authentication key to use.</param>
        public WampCraClientAuthenticator(string authenticationId, string authenticationKey)
        {
            mAuthenticationId = authenticationId;
            mAuthenticationKey = authenticationKey;
        }

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
                                          string salt,
                                          int? iterations = null,
                                          int? keyLen = null)
        {
            mAuthenticationId = authenticationId;
            mAuthenticationKey = WampCraHelpers.DeriveKey(secret, salt, iterations, keyLen);
        }

        public AuthenticationResponse Authenticate(string authmethod, ChallengeDetails extra)
        {
            if (authmethod == WAMP_CRA)
            {
                WampCraChallengeDetails challengeDetails = 
                    extra.OriginalValue.Deserialize<WampCraChallengeDetails>();
                
                string signature =
                    WampCraHelpers.Sign(mAuthenticationKey, challengeDetails.Challenge);

                AuthenticationResponse result =
                    new AuthenticationResponse { Signature = signature };

                return result;
            }
            else
            {
                throw new WampAuthenticationException("don't know how to authenticate using '" + authmethod + "'");
            }
        }

        public string[] AuthenticationMethods
        {
            get
            {
                return new string[] {WAMP_CRA};
            }
        }

        public string AuthenticationId
        {
            get
            {
                return mAuthenticationId;
            }
        }

        internal class WampCraChallengeDetails
        {
            [PropertyName("challenge")]
            public string Challenge { get; set; }
        }
    }
}