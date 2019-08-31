using System;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Occurs when an CHALLENGE message has been received, 
    /// but no <see cref="IWampClientAuthenticator"/> has been provided.
    /// </summary>
    [Serializable]
    public class WampAuthenticationNotImplementedException : WampAuthenticationException
    {
        /// <summary>
        /// Initializes an new instance of <see cref="WampAuthenticationNotImplementedException"/>.
        /// </summary>
        public WampAuthenticationNotImplementedException(string message = DefaultMessage, string reason = WampErrors.WampErrorCannotAuthenticate) : base(message, reason)
        {
        }
    }
}