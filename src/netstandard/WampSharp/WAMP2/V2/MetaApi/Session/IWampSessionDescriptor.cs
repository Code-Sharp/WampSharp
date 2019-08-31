using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSessionDescriptor
    {
        /// <summary>
        /// Obtains the number of sessions currently attached to the realm.
        /// </summary>
        /// <returns>The number of sessions currently attached to the realm.</returns>
        [WampProcedure("wamp.session.count")]
        long CountSessions();

        /// <summary>
        /// Retrieves a list of the session IDs for all sessions currently attached to the realm.
        /// </summary>
        /// <returns>List of WAMP session IDs (order undefined).</returns>
        [WampProcedure("wamp.session.list")]
        long[] GetAllSessionIds();

        /// <summary>
        /// Retrieves information on a specific session.
        /// </summary>
        /// <param name="sessionId">The session ID of the session to retrieve details for.</param>
        /// <returns>Information on the particular session.</returns>
        [WampProcedure("wamp.session.get")]
        WampSessionDetails GetSessionDetails(long sessionId);
    }
}