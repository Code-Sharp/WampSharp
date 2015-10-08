using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents an interface that can respond to a router's CHALLENGE message..
    /// </summary>
    public interface IWampClientAuthenticator
    {
        /// <summary>
        /// Occurs when server sends a CHALLENGE message.
        /// </summary>
        /// <param name="authmethod">The authentication method the server declared it's using.</param>
        /// <param name="extra">The challenge details sent with this request.</param>
        /// <returns>A <see cref="AuthenticationResponse"/> - an object representing the
        /// AUTHENTICATE message.</returns>
        /// <exception cref="WampAuthenticationException">If can't respond to CHALLENGE.</exception>
        AuthenticationResponse Authenticate(string authmethod, ChallengeDetails extra);

        /// <summary>
        /// Gets an array of the authentication methods this client supports.
        /// These will be sent upon HELLO message.
        /// </summary>
        string[] AuthenticationMethods
        {
            get;
        }

        /// <summary>
        /// Gets the authid of this client. This will be sent upon HELLO message.
        /// </summary>
        string AuthenticationId
        {
            get;
        }
    }
}