using System.Collections.Generic;

namespace WampSharp.V2.Authentication
{
    public class WampCraStaticUserDb : IWampCraUserDb
    {
        private readonly IDictionary<string, WampCraUser> mUsers;

        public WampCraStaticUserDb(IDictionary<string, WampCraUser> users)
        {
            mUsers = users;
        }

        public WampCraUser GetUserById(string authenticationId)
        {
            WampCraUser result;
            mUsers.TryGetValue(authenticationId, out result);
            return result;
        }
    }
}