namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Represents a container of <see cref="IWampHostedRealm"/>.
    /// </summary>
    public interface IWampHostedRealmContainer
    {
        /// <summary>
        /// Gets a realm by its name.
        /// </summary>
        /// <param name="name">The requested ream name.</param>
        /// <returns>The request realm.</returns>
        IWampHostedRealm GetRealmByName(string name);         
    }
}