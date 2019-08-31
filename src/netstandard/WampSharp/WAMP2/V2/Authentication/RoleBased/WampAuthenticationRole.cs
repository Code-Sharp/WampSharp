namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An object representing an authetication role.
    /// </summary>
    public class WampAuthenticationRole
    {
        /// <summary>
        /// Gets or sets the role name. This will be sent as authrole upon WELCOME message.
        /// If null, the <see cref="IWampAuthenticationProvider.GetRoleByName"/>'s
        /// role parameter is used.
        /// </summary>
        public string AuthenticationRole { get; set; }

        /// <summary>
        /// Gets or sets the authentication provider's name. This will be sent as authprovider upon WELCOME message.
        /// If null, <see cref="IWampAuthenticationProvider.ProviderName"/>
        /// is used instead.
        /// </summary>
        public string AuthenticationProvider { get; set; }

        /// <summary>
        /// Gets the <see cref="IWampAuthorizer"/> used to authorize the role's actions.
        /// </summary>
        public IWampAuthorizer Authorizer { get; set; }
    }
}