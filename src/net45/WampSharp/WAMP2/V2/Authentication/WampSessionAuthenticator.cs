using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An abstract class for <see cref="IWampSessionAuthenticator"/>.
    /// </summary>
    public abstract class WampSessionAuthenticator : IWampSessionAuthenticator
    {
        private static readonly ChallengeDetails mEmptyChallengeDetails = new ChallengeDetails();
        private ChallengeDetails mChallengeDetails;

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected WampSessionAuthenticator()
        {
            mChallengeDetails = mEmptyChallengeDetails;
        }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.IsAuthenticated"/>
        /// </summary>
        public virtual bool IsAuthenticated { get; protected set; }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.Authorizer"/>
        /// </summary>
        public virtual IWampAuthorizer Authorizer { get; protected set; }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.Authenticate"/>
        /// </summary>
        public abstract void Authenticate(string signature, AuthenticateExtraData extra);

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.AuthenticationId"/>
        /// </summary>
        public abstract string AuthenticationId { get; }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.AuthenticationMethod"/>
        /// </summary>
        public abstract string AuthenticationMethod { get; }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.ChallengeDetails"/>. 
        /// This method should be overriden for CHALLENGE details customization.
        /// </summary>
        public virtual ChallengeDetails ChallengeDetails
        {
            get => mChallengeDetails;
            protected set => mChallengeDetails = value;
        }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.WelcomeDetails"/>. 
        /// This method should be overriden for WELCOME details customization.
        /// </summary>
        public virtual WelcomeDetails WelcomeDetails
        {
            get;
            protected set;
        }
    }

    /// <summary>
    /// An abstract class for <see cref="IWampSessionAuthenticator"/>, with typed <see cref="AuthenticateExtraData"/>.
    /// </summary>
    /// <typeparam name="TExtra"></typeparam>
    public abstract class WampSessionAuthenticator<TExtra> : WampSessionAuthenticator
    {
        public sealed override void Authenticate(string signature, AuthenticateExtraData extra)
        {
            TExtra deserialized = extra.OriginalValue.Deserialize<TExtra>();

            Authenticate(signature, deserialized);
        }

        /// <summary>
        /// <see cref="IWampSessionAuthenticator.Authenticate"/>
        /// </summary>
        protected abstract void Authenticate(string signature, TExtra extra);
    }
}