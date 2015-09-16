#if !PCL
using System;
using WampSharp.Core.Cra;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
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
                WampCraHelpers.AuthSignature(AuthenticationChallenge, Secret, CraChallengeDetails);

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

        public override sealed ChallengeDetails ChallengeDetails
        {
            get
            {
                return CraChallengeDetails;
            }
            protected set
            {
                throw new Exception("Use CraChallengeDetails property instead.");
            }
        }

        public abstract string AuthenticationChallenge { get; }
        
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
            set
            {
                mCraChallengeDetails = value;
            }
        }

        public override string AuthenticationId
        {
            get
            {
                return mAuthenticationId;
            }
        }

        public override string AuthenticationMethod
        {
            get
            {
                return "wampcra";
            }
        }
    }
}
#endif