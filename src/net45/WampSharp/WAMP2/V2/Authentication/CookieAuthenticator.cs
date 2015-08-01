using System.Collections.Generic;
using System.Net;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public abstract class CookieAuthenticator : IWampSessionAuthenticator
    {
        private ICookieProvider mCookieProvider;
        public abstract bool IsAuthenticated { get; }
        public abstract string AuthenticationId { get; }
        public abstract string AuthenticationMethod { get; }
        public abstract ChallengeDetails ChallengeDetails { get; }
        public abstract void Authenticate(string signature, AuthenticateExtraData extra);
        public abstract IWampAuthorizer Authorizer { get; }
        public abstract WelcomeDetails WelcomeDetails { get; }

        protected CookieAuthenticator(ICookieProvider cookieProvider)
        {
            mCookieProvider = cookieProvider;
        }

        protected Cookie GetCookieByName(string cookieName)
        {
            return mCookieProvider.GetCookieByName(cookieName);
        }

        protected IEnumerable<Cookie> AvailableCookies
        {
            get
            {
                return mCookieProvider.Cookies;
            }
        }
    }

    public interface ICookieAuthenticatorFactory
    {
        IWampSessionAuthenticator CreateAuthenticator(ICookieProvider cookieProvider);
    }

    public interface ICookieProvider
    {
        IEnumerable<Cookie> Cookies { get; }
        Cookie GetCookieByName(string cookieName);
    }
}