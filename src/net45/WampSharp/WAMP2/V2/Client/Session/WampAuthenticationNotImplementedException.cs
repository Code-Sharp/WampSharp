using System;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Occurs when an CHALLENGE message has been received, 
    /// but no <see cref="IWampClientAuthenticator"/> has been provided.
    /// </summary>
#if !PCL
    [Serializable]
#endif
    public class WampAuthenticationNotImplementedException : WampAuthenticationException
    {
        /// <summary>
        /// Initializes an new instance of <see cref="WampAuthenticationNotImplementedException"/>.
        /// </summary>
        public WampAuthenticationNotImplementedException(string message = DefaultMessage, string reason = WampErrorCannotAuthenticate) : base(message, reason)
        {
        }
    }
}