namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Contains predefined WAMP ERROR uris
    /// </summary>
    /// <remarks>
    /// From https://github.com/tavendo/WAMP/blob/master/spec/basic.md
    /// </remarks>
    public static class WampErrors
    {
        /// <summary>
        /// *Peer* provided an incorrect URI for any URI-based attribute of WAMP message, such as realm, topic or procedure
        /// </summary>
        public const string InvalidUri = "wamp.error.invalid_uri";

        /// <summary>
        /// Indicates that the close was with a normal reason.
        /// </summary>
        public const string CloseNormal = "wamp.close.normal";

        /// <summary>
        /// *Peer* is not authorized to access the given resource. This might be triggered by a session trying to join a realm, a publish, subscribe, register or call.
        /// </summary>
        public const string NotAuthorized = "wamp.error.not_authorized";

        /// <summary>
        /// *Peer* wanted to join a non-existing realm (and the *Router* did not allow to auto-create the realm).
        /// </summary>
        public const string NoSuchRealm = "wamp.error.no_such_realm";

        /// <summary>
        /// The *Peer* is shutting down completely - used as a `GOODBYE` (or `ABORT`) reason.
        /// </summary>
        public const string SystemShutdown = "wamp.error.system_shutdown";

        /// <summary>
        /// The *Peer* want to leave the realm - used as a `GOODBYE` reason.
        /// </summary>
        public const string CloseRealm = "wamp.error.close_realm";

        /// <summary>
        /// A *Peer* acknowledges ending of a session - used as a `GOOBYE` reply reason.
        /// </summary>
        public const string GoodbyeAndOut = "wamp.error.goodbye_and_out";

        /// <summary>
        /// A *Dealer* could not perform a call, since the procedure called does not exist.
        /// </summary>
        public const string NoSuchProcedure = "wamp.error.no_such_procedure";

        /// <summary>
        /// A *Broker* could not perform a unsubscribe, since the given subscription is not active.
        /// </summary>
        public const string NoSuchSubscription = "wamp.error.no_such_subscription";

        /// <summary>
        /// A *Dealer* could not perform a unregister, since the given registration is not active.
        /// </summary>
        public const string NoSuchRegistration = "wamp.error.no_such_registration";

        /// <summary>
        /// A call failed, since the given argument types or values are not acceptable to the called procedure.
        /// </summary>
        public const string InvalidArgument = "wamp.error.invalid_argument";

        /// <summary>
        /// A publish failed, since the given topic is not acceptable to the *Broker*.
        /// </summary>
        public const string InvalidTopic = "wamp.error.invalid_topic";

        /// <summary>
        /// A procedure could not be registered, since a procedure with the given URI is already registered (and the *Dealer* is not able to set up a distributed registration).
        /// </summary>
        public const string ProcedureAlreadyExists = "wamp.error.procedure_already_exists";         
    }
}