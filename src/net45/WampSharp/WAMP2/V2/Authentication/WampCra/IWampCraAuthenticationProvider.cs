namespace WampSharp.V2.Authentication
{
    public interface IWampCraAuthenticationProvider
    {
        string ProviderName { get; }

        WampCraAuthenticationRole GetRoleByName(string realm, string role);
    }
}