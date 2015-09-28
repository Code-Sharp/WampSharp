namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents permissions to a given uri.
    /// Used by <see cref="WampStaticAuthorizer"/>
    /// </summary>
    public class WampUriPermissions
    {
        /// <summary>
        /// Gets or sets the uri these permissions belong to.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the uri is prefixed or not.
        /// </summary>
        public bool Prefixed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether PUBLISH action
        /// is allowed to this uri.
        /// </summary>
        public bool CanPublish { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SUBSCRIBE action
        /// is allowed to this uri.
        /// </summary>
        public bool CanSubscribe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CALL action
        /// is allowed to this uri.
        /// </summary>
        public bool CanCall { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether REGISTER action
        /// is allowed to this uri.
        /// </summary>
        public bool CanRegister { get; set; }
    }
}