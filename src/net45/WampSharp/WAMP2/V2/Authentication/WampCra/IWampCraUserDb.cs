namespace WampSharp.V2.Authentication
{
    public interface IWampCraUserDb
    {
        WampCraUser GetUserById(string authenticationId);
    }
}