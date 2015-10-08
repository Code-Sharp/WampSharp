namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a factory for <see cref="IWampSessionAuthenticator"/>.
    /// </summary>
    public interface IWampSessionAuthenticatorFactory
    {
        /// <summary>
        /// Creates a <see cref="IWampSessionAuthenticator"/> for a pending client.
        /// </summary>
        /// <param name="details">The client's details.</param>
        /// <param name="transportAuthenticator">The client's transport </param>
        /// <returns>The <see cref="IWampSessionAuthenticator"/> created for the given client.</returns>
        IWampSessionAuthenticator GetSessionAuthenticator
            (WampPendingClientDetails details,
             IWampSessionAuthenticator transportAuthenticator);
    }
}