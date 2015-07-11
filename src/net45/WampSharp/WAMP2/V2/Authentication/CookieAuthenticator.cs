using System.Collections.Generic;
using System.Net;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public abstract class CookieAuthenticator : IWampSessionAuthenticator
    {
        public abstract bool IsAuthenticated { get; }
        public abstract string AuthenticationId { get; }
        public abstract string AuthenticationMethod { get; }
        public abstract ChallengeDetails Details { get; }
        public abstract void Authenticate(string signature, AuthenticateExtraData extra);
        public abstract IWampAuthorizer Authorizer { get; }
        internal ICookieProvider CookieProvider { get; set; }

        protected CookieAuthenticator(ICookieProvider cookieProvider)
        {
            CookieProvider = cookieProvider;
        }

        protected Cookie GetCookieByName(string cookieName)
        {
            return CookieProvider.GetCookieByName(cookieName);
        }

        protected IEnumerable<Cookie> AvailableCookies
        {
            get
            {
                return CookieProvider.Cookies;
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