namespace WampSharp.V2.Authentication
{
    public class WampAuthenticationRole
    {
        public string AuthenticationRole { get; set; }

        public string AuthenticationProvider { get; set; }

        public IWampAuthorizer Authorizer { get; set; }
    }
}