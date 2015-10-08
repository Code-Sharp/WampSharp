namespace WampSharp.V2.Authentication
{
    internal class RestrictedSessionAuthenticationFactory : IWampSessionAuthenticatorFactory
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public RestrictedSessionAuthenticationFactory(IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public IWampSessionAuthenticator GetSessionAuthenticator
            (WampPendingClientDetails details,
             IWampSessionAuthenticator transportAuthenticator)
        {
            IWampSessionAuthenticator result =
                mSessionAuthenticationFactory.GetSessionAuthenticator
                    (details,
                     transportAuthenticator);

            if (result == null)
            {
                return null;
            }
            
            return new RestrictedSessionAuthenticator(result);
        }
    }
}