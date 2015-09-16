namespace WampSharp.V2.Authentication
{
    public class WampCraAuthenticationRole
    {
        public string AuthenticationRole { get; set; }

        public string AuthenticationProvider { get; set; }

        public IWampAuthorizer Authorizer { get; set; }
    }
}