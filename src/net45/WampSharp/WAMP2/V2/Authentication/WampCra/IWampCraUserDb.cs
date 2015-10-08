namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a database of WampCra users.
    /// </summary>
    public interface IWampCraUserDb
    {
        /// <summary>
        /// Gets the requetsed user's details given its authentication id.
        /// </summary>
        /// <param name="authenticationId">The requested user's authentication id.</param>
        /// <returns>The user's WAMP-CRA authentication details.</returns>
        WampCraUser GetUserById(string authenticationId);
    }
}