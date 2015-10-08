using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a WAMP session authenticator - i.e. a mechanism that performs the authentication
    /// process of an individual client.
    /// </summary>
    public interface IWampSessionAuthenticator
    {
        /// <summary>
        /// Gets a value indicating whether the client is already authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the authentication id of the authenticated client.
        /// This will be sent upon WELCOME message (in details.auth_id)
        /// </summary>
        string AuthenticationId { get; }

        /// <summary>
        /// Gets the authentication method used for authentication.
        /// (This will be sent upon CHALLENGE and WELCOME messages in details.auth_method)
        /// </summary>
        string AuthenticationMethod { get; }

        /// <summary>
        /// Gets the challenge details for the given client authentication process. 
        /// (This will be sent upon CHALLENGE message)
        /// </summary>
        ChallengeDetails ChallengeDetails { get; }

        /// <summary>
        /// Occurs when a client responds to a CHALLENGE message with an AUTHENTICATE message.
        /// This should set <see cref="IsAuthenticated"/> to true if the client passed authentication,
        /// and can throw <see cref="WampAuthenticationException"/> if client isn't authenticated.
        /// </summary>
        /// <param name="signature">The signature received upon AUTHENTICATE message.</param>
        /// <param name="extra">The extra data received upon AUTHENTICATE message.</param>
        void Authenticate(string signature, AuthenticateExtraData extra);

        /// <summary>
        /// Gets the authorizer to use after the client is authenticated.
        /// </summary>
        IWampAuthorizer Authorizer { get; }

        /// <summary>
        /// Gets the WELCOME details to send to client after successful authentication.
        /// </summary>
        WelcomeDetails WelcomeDetails { get; }
    }
}