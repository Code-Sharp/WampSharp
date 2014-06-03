namespace WampSharp.Cra
{
    internal class WampCraPendingAuth
    {
        public WampCraPendingAuth(WampCraChallenge authInfo, string signature, WampCraPermissions permissions)
        {
            AuthInfo = authInfo;
            Signature = signature;
            Permissions = permissions;
        }

        public WampCraChallenge AuthInfo { get; private set; }
        public string Signature { get; private set; }
        public WampCraPermissions Permissions { get; private set; }
    }
}
