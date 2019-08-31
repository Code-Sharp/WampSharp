using WampSharp.V2.Authentication;

namespace WampSharp.Core.Listener
{
    public abstract class AsyncWebSocketWampConnection<TMessage> : 
        AsyncWampConnection<TMessage>,
        IWampAuthenticatedConnection<TMessage>
    {
        public AsyncWebSocketWampConnection
            (ICookieProvider cookieProvider,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null)
        {
            if (cookieAuthenticatorFactory != null)
            {
                IWampSessionAuthenticator authenticator =
                    cookieAuthenticatorFactory.CreateAuthenticator(cookieProvider);

                Authenticator = authenticator;
            }
        }

        public IWampSessionAuthenticator Authenticator { get; }
    }
}