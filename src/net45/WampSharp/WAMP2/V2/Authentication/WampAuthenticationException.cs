using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An exception that can be thrown if can't authenticate with router.
    /// This sends an ABORT message to the router.
    /// </summary>
    [Serializable]
    public class WampAuthenticationException : Exception
    {
        protected const string DefaultMessage = "sorry, I cannot authenticate (onchallenge handler raised an exception)";

        /// <summary>
        /// Initializes an new instance of <see cref="WampAuthenticationException"/>
        /// </summary>
        /// <param name="message">The message to send with the details of the ABORT message.</param>
        /// <param name="reason">The reason to send with the ABORT message.</param>
        public WampAuthenticationException(
            string message = DefaultMessage,
            string reason = WampErrors.WampErrorCannotAuthenticate)
            : this(new AbortDetails {Message = message}, reason)
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="WampAuthenticationException"/>
        /// </summary>
        /// <param name="details">The details to send with the ABORT message.</param>
        /// <param name="reason">The reason to send with the ABORT message.</param>
        public WampAuthenticationException(AbortDetails details, string reason = WampErrors.WampErrorCannotAuthenticate)
            : base(details.Message)
        {
            Details = details;
            Reason = reason;
        }

        /// <summary>
        /// Gets the details to send with the ABORT message.
        /// </summary>
        public AbortDetails Details { get; }

        /// <summary>
        /// Gets the reason to send with the ABORT message.
        /// </summary>
        public string Reason { get; }
    }
}