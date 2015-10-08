using WampSharp.V2.Authentication;

namespace WampSharp.V2.Transports
{
    /// <summary>
    /// Represents a WebSocket transport.
    /// </summary>
    public abstract class WebSocketTransport<TConnection> : TextBinaryTransport<TConnection>
    {
        private readonly ICookieAuthenticatorFactory mAuthenticatorFactory;

        public WebSocketTransport(ICookieAuthenticatorFactory authenticatorFactory)
        {
            mAuthenticatorFactory = authenticatorFactory;
        }

        #region Protected Members

        protected ICookieAuthenticatorFactory AuthenticatorFactory
        {
            get
            {
                return mAuthenticatorFactory;
            }
        }

        #endregion
   }
}