using System.Collections.Generic;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a static data based implementation of <see cref="IWampCraUserDb"/>.
    /// </summary>
    public class WampCraStaticUserDb : IWampCraUserDb
    {
        private readonly IDictionary<string, WampCraUser> mUsers;

        /// <summary>
        /// Instantiates a new instance of <see cref="WampCraStaticUserDb"/> given
        /// a map of authentication id to WAMP-CRA user details.
        /// </summary>
        /// <param name="users">The given map of authentication id to WAMP-CRA user details.</param>
        public WampCraStaticUserDb(IDictionary<string, WampCraUser> users)
        {
            mUsers = users;
        }

        public WampCraUser GetUserById(string authenticationId)
        {
            mUsers.TryGetValue(authenticationId, out WampCraUser result);
            return result;
        }
    }
}