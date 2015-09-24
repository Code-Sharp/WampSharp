namespace WampSharp.V2.Authentication
{
    public interface IWampAuthenticationProvider
    {
        string ProviderName { get; }

        WampAuthenticationRole GetRoleByName(string realm, string role);
    }
}