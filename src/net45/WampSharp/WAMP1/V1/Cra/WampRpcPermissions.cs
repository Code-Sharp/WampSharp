
namespace WampSharp.V1.Cra
{
    /// <summary>
    /// Represents RPC call permissions for an endpoint uri.
    /// </summary>
    /// <remarks>As this is defined as part of WAMP-CRA (v1), it should not be changed.</remarks>
    public class WampRpcPermissions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uri">RPC Endpoint URI.</param>
        /// <param name="call">Allow to call?</param>
        public WampRpcPermissions(string uri, bool call)
        {
            this.uri = uri;
            this.call = call;
        }

        /// <summary>
        /// RPC Endpoint URI.
        /// </summary>
        public string uri { get; private set; }

        /// <summary>
        /// Allow to call?
        /// </summary>
        public bool call { get; private set; }
    }
}
