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
        /// The application payload could not be serialized.
        /// </summary>
        public const string InvalidPayload = "wamp.error.invalid_payload";

        /// <summary>
        /// Indicates that the close was with a normal reason.
        /// </summary>
        public const string CloseNormal = "wamp.close.normal";

        /// <summary>
        /// *Peer* is not authorized to access the given resource. This might be triggered by a session trying to join a realm, a publish, subscribe, register or call.
        /// </summary>
        public const string NotAuthorized = "wamp.error.not_authorized";

        /// <summary>
        /// A Dealer or Broker could not determine if the *Peer* is authorized to perform
        /// a join, call, register, publish or subscribe, since the authorization operation
        /// *itself* failed. E.g. a custom authorizer did run into an error.
        /// </summary>
        public const string AuthorizationFailed = "wamp.error.authorization_failed";

        /// <summary>
        /// *Peer* wanted to join a non-existing realm (and the *Router* did not allow to auto-create the realm).
        /// </summary>
        public const string NoSuchRealm = "wamp.error.no_such_realm";

        /// <summary>
        /// A *Peer* was to be authenticated under a Role that does not (or no longer) exists on the Router.
        /// For example, the *Peer* was successfully authenticated, but the Role configured does not
        /// exists - hence there is some misconfiguration in the Router.
        /// </summary>
        public const string NoSuchRole = "wamp.error.no_such_role";

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
        /// A *Dealer* could not perform a cancellation, since the given invocation does not exist.
        /// </summary>
        public const string NoSuchInvocation = "wamp.error.no_such_invocation";

        /// <summary>
        /// A router could not perform an operation, since a session ID specified was non-existant.
        /// </summary>
        public const string NoSuchSession = "wamp.error.no_such_session";

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

        /// <summary>
        /// A procedure could not be registered, since a procedure with the given URI is 
        /// already registered, and the registration has a conflicting invocation policy.
        /// </summary>
        public const string ProcedureExistsInvocationPolicyConflict = "wamp.error.procedure_exists_with_different_invocation_policy";

        public const string WampErrorCannotAuthenticate = "wamp.error.cannot_authenticate";

        // TODO: Custom uris that should be part of the WAMP spec.
        public const string CalleeDisconnected = Canceled;

        public const string InvalidOptions = "wamp.error.invalid_options";

        public const string CalleeUnregistered = "wamp.error.callee_unregistered";
        public const string DiscloseMeNotAllowed = "wamp.error.disclose_me.not_allowed";

        /// <summary>
        /// A Dealer or Callee canceled a call previously issued (WAMP AP).
        /// </summary>
        public const string Canceled = "wamp.error.canceled";
    }
}