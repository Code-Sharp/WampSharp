using WampSharp.Core.Cra;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents details of a WAMP-CRA user.
    /// </summary>
    public class WampCraUser : IWampCraChallenge
    {
        /// <summary>
        /// Gets the user's authentication id. If null, uses HELLO.details.authid.
        /// </summary>
        public string AuthenticationId { get; set; }

        /// <summary>
        /// Gets the user's authentication role.
        /// </summary>
        public string AuthenticationRole { get; set; }

        /// <summary>
        /// Gets the user's secret. If salted, contains the derived key (and NOT the actual secret).
        /// </summary>
        public string Secret { get; set; }

        public string Salt { get; set; }

        public int? Iterations { get; set; }

        public int? KeyLength { get; set; }
    }
}