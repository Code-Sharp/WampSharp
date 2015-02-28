using System.Collections.Generic;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents an AUTHENTICATE message to send to the router in response to a
    /// corresponding CHALLENGE message.
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// Gets the signature to send.
        /// </summary>
        public string Signature
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the extra data to send.
        /// </summary>
        public IDictionary<string, object> Extra
        {
            get; 
            set;
        }
    }
}