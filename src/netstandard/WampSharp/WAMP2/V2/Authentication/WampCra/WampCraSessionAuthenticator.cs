using System;
using WampSharp.Core.Cra;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An abstract class for an implementation of a WAMP-CRA <see cref="IWampSessionAuthenticator"/>.
    /// </summary>
    public abstract class WampCraSessionAuthenticator : WampSessionAuthenticator
    {
        private readonly string mAuthenticationId;
        private WampCraChallengeDetails mCraChallengeDetails;

        protected WampCraSessionAuthenticator(string authenticationId)
        {
            mAuthenticationId = authenticationId;
        }

        public override void Authenticate(string signature, AuthenticateExtraData extra)
        {
            string computedSignature =
                WampCraHelpers.Sign(Secret, AuthenticationChallenge);

            if (computedSignature == signature)
            {
                IsAuthenticated = true;
            }
            else
            {
                throw new WampAuthenticationException("signature is invalid",
                                                      WampErrors.NotAuthorized);
            }
        }

        public sealed override ChallengeDetails ChallengeDetails
        {
            get => CraChallengeDetails;
            protected set => throw new Exception("Use CraChallengeDetails property instead.");
        }

        /// <summary>
        /// Gets the authentication challenge - this is the challenge that will be sent upon CHALLENGE
        /// message
        /// </summary>
        public abstract string AuthenticationChallenge { get; }
        
        /// <summary>
        /// Gets the secret used to compute the signature.
        /// </summary>
        public abstract string Secret { get; }

        protected WampCraChallengeDetails CraChallengeDetails
        {
            get
            {
                WampCraChallengeDetails result =
                    mCraChallengeDetails ?? new WampCraChallengeDetails();

                result.Challenge = AuthenticationChallenge;
                
                return result;
            }
            set => mCraChallengeDetails = value;
        }

        public override string AuthenticationId => mAuthenticationId;

        public override string AuthenticationMethod => WampAuthenticationMethods.WampCra;
    }
}