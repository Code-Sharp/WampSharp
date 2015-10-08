using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSessionDescriptorProxy : IWampSessionDescriptor
    {
        /// <summary>
        /// Obtains the number of sessions currently attached to the realm.
        /// </summary>
        /// <returns>The number of sessions currently attached to the realm.</returns>
        [WampProcedure("wamp.session.count")]
        Task<long> CountSessionsAsync();

        /// <summary>
        /// Retrieves a list of the session IDs for all sessions currently attached to the realm.
        /// </summary>
        /// <returns>List of WAMP session IDs (order undefined).</returns>
        [WampProcedure("wamp.session.list")]
        Task<long[]> GetAllSessionIdsAsync();

        /// <summary>
        /// Retrieves information on a specific session.
        /// </summary>
        /// <param name="sessionId">The session ID of the session to retrieve details for.</param>
        /// <returns>Information on the particular session.</returns>
        [WampProcedure("wamp.session.get")]
        Task<WampSessionDetails> GetSessionDetailsAsync(long sessionId);
    }
}