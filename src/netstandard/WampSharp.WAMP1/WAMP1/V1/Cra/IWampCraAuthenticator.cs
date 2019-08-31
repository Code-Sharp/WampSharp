namespace WampSharp.V1.Cra
{
    /// <summary>
    /// Interface to data regarding a client that is authenticated (or being authenticated).
    /// </summary>
    public interface IWampCraAuthenticator
    {
        /// <summary>
        /// Gets the sessionId of the connected client.
        /// </summary>
        string ClientSessionId { get; }

        /// <summary>
        /// The authKey provided by the client during the WAMP-CRA authentication request.
        /// </summary>
        string AuthKey { get; }

        /// <summary>
        /// Gets a value indicating whether the user identified by AuthKey is successfully authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}