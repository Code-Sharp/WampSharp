namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a factory for <see cref="IWampSessionAuthenticator"/> based on cookies.
    /// </summary>
    public interface ICookieAuthenticatorFactory
    {
        /// <summary>
        /// Creates a <see cref="IWampSessionAuthenticator"/> given a <see cref="ICookieProvider"/>.
        /// </summary>
        /// <param name="cookieProvider">The given <see cref="ICookieProvider"/>.</param>
        /// <returns>The created <see cref="IWampSessionAuthenticator"/>.</returns>
        IWampSessionAuthenticator CreateAuthenticator(ICookieProvider cookieProvider);
    }
}