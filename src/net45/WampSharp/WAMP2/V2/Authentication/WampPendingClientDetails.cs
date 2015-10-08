using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents details of a client pending for authentication.
    /// </summary>
    public class WampPendingClientDetails
    {
        /// <summary>
        /// Gets the pending client's HELLO message details.
        /// </summary>
        public HelloDetails HelloDetails { get; internal set; }

        /// <summary>
        /// Gets the pending client's session id.
        /// </summary>
        public long SessionId { get; internal set; }

        /// <summary>
        /// Gets the pending client's requested realm (sent upon HELLO message).
        /// </summary>
        public string Realm { get; internal set; }
    }
}