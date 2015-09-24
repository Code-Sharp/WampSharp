namespace WampSharp.V2.Authentication
{
    public class WampUriPermissions
    {
        public string Uri { get; set; }

        public bool Prefixed { get; set; }

        public bool CanPublish { get; set; }

        public bool CanSubscribe { get; set; }

        public bool CanCall { get; set; }

        public bool CanRegister { get; set; }
    }
}