namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a role-based authentication provider.
    /// </summary>
    public interface IWampAuthenticationProvider
    {
        /// <summary>
        /// Gets the provider name. 
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Gets an object representing the role given the realm and the role name.
        /// </summary>
        /// <param name="realm">The given realm to search to role in.</param>
        /// <param name="role">The given name of the role to search.</param>
        /// <returns>An object representing the requested role, or null if no such found.</returns>
        WampAuthenticationRole GetRoleByName(string realm, string role);
    }
}