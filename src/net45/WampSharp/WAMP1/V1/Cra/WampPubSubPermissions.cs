
namespace WampSharp.V1.Cra
{
    /// <summary>
    /// Represents Publish and Subscribe permissions for a Topic URI or URI Prefix.
    /// </summary>
    /// <remarks>As this is defined as part of WAMP-CRA (v1), it should not be changed.</remarks>
    public class WampPubSubPermissions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="uri">PubSub Topic URI / URI prefix.</param>
        /// <param name="prefix">URI matched by prefix?</param>
        /// <param name="pub">Allow to publish?</param>
        /// <param name="sub">Allow to subscribe?</param>
        public WampPubSubPermissions(string uri, bool prefix, bool pub, bool sub)
        {
            this.uri = uri;
            this.prefix = prefix;
            this.pub = pub;
            this.sub = sub;
        }

        /// <summary>
        /// PubSub Topic URI / URI prefix.
        /// </summary>
        public string uri { get; private set; }

        /// <summary>
        /// URI matched by prefix?
        /// </summary>
        public bool prefix { get; private set; }

        /// <summary>
        /// Allow to publish?
        /// </summary>
        public bool pub { get; private set; }

        /// <summary>
        /// Allow to subscribe?
        /// </summary>
        public bool sub { get; private set; }
    }
}
