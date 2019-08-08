using System;
using System.Threading.Tasks;
using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public class SessionEvents : MetaApiEventsBase<IWampSessionMetadataSubscriber>
    {
        /// <summary>
        /// Fired when a session joins a realm on the router.
        /// </summary>
        /// <param name="details">An object describing the joined session.</param>
        public delegate void OnJoinDelegate(WampSessionDetails details);

        /// <summary>
        /// Fired when a session leaves a realm on the router or is disconnected.
        /// </summary>
        /// <param name="sessionId">The session ID of the session that left.</param>
        public delegate void OnLeaveDelegate(long sessionId);

        public SessionEvents(IWampRealmProxy realmProxy) : base(realmProxy)
        {
        }

        public Task<IAsyncDisposable> OnJoin(OnJoinDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnJoin(default(WampSessionDetails)));
        }

        public Task<IAsyncDisposable> OnLeave(OnLeaveDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnLeave(default(long)));
        }
    }
}