using WampSharp.Core.Cra;

namespace WampSharp.V2.Authentication
{
    public class WampCraUser : IWampCraChallenge
    {
        public string AuthenticationId { get; set; }

        public string AuthenticationRole { get; set; }

        public string Secret { get; set; }

        public string Salt { get; set; }

        public int? Iterations { get; set; }

        public int? KeyLength { get; set; }
    }
}