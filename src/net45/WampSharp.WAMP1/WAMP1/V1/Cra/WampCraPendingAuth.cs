namespace WampSharp.V1.Cra
{
    internal class WampCraPendingAuth
    {
        public WampCraPendingAuth(WampCraChallenge authInfo, string signature, WampCraPermissions permissions)
        {
            AuthInfo = authInfo;
            Signature = signature;
            Permissions = permissions;
        }

        public WampCraChallenge AuthInfo { get; }
        public string Signature { get; }
        public WampCraPermissions Permissions { get; }
    }
}
