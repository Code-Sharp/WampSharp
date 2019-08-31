using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Management
{
    public interface IWampSessionManagementService
    {
        /// <summary>
        /// Kills a single session identified by session ID.
        /// The caller of this meta procedure may only specify session IDs other than its own session.  Specifying the caller's own session will result in a `wamp.error.no_such_session` since no _other_ session with that ID exists.
        /// The keyword arguments are optional, and if not provided the reason defaults to `wamp.close.normal` and the message is omitted from the `GOODBYE` sent to the closed session. 
        /// </summary>
        /// <param name="session">The session ID of the session to close.</param>
        /// <param name="reason">reason for closing session, sent to client in GOODBYE.Reason.</param>
        /// <param name="message">additional information sent to client in GOODBYE.Details under the key "message".</param>
        [WampProcedure("wamp.session.kill")]
        void KillBySessionId(long session, string reason = WampErrors.CloseNormal, string message = null);

        /// <summary>
        /// Kills all currently connected sessions that have the specified `authid`.
        /// If the caller's own session has the specified `authid`, the caller's session is excluded from the closed sessions.
        /// The keyword arguments are optional, and if not provided the reason defaults to `wamp.close.normal` and the message is omitted from the `GOODBYE` sent to the closed session.
        /// </summary>
        /// <param name="authId">The authentication ID identifying sessions to close.</param>
        /// <param name="reason">reason for closing sessions, sent to clients in GOODBYE.Reason</param>
        /// <param name="message">additional information sent to clients in GOODBYE.Details under the key "message".</param>
        /// <returns>The number of sessions closed by this meta procedure.</returns>
        [WampProcedure("wamp.session.kill_by_authid")]
        int KillByAuthId(string authId, string reason = WampErrors.CloseNormal, string message = null);

        /// <summary>
        /// Kills all currently connected sessions that have the specified `authrole`.
        /// If the caller's own session has the specified `authrole`, the caller's session is excluded from the closed sessions.
        /// The keyword arguments are optional, and if not provided the reason defaults to `wamp.close.normal` and the message is omitted from the `GOODBYE` sent to the closed session.        [WampProcedure("wamp.session.kill_by_authrole")]
        /// </summary>
        /// <param name="authRole">The authentication role identifying sessions to close.</param>
        /// <param name="reason">reason for closing sessions, sent to clients in GOODBYE.Reason.</param>
        /// <param name="message">additional information sent to clients in `GOODBYE.Details` under the key "message".</param>
        /// <returns>The number of sessions closed by this meta procedure.</returns>
        [WampProcedure("wamp.session.kill_by_authrole")]
        int KillByAuthRole(string authRole, string reason = WampErrors.CloseNormal, string message = null);

        /// <summary>
        /// Kills all currently connected sessions in the caller's realm.
        /// The caller's own session is excluded from the closed sessions.  Closing all sessions in the realm will not generate session meta events or testament events, since no subscribers would remain to receive these events.
        /// The keyword arguments are optional, and if not provided the reason defaults to `wamp.close.normal` and the message is omitted from the `GOODBYE` sent to the closed session.
        /// </summary>
        /// <param name="reason">reason for closing sessions, sent to clients in `GOODBYE.Reason`.</param>
        /// <param name="message">additional information sent to clients in `GOODBYE.Details` under the key "message".</param>
        /// <returns>The number of sessions closed by this meta procedure.</returns>
        [WampProcedure("wamp.session.kill_all")]
        int KillAll(string reason = WampErrors.CloseNormal, string message = null);
    }
}