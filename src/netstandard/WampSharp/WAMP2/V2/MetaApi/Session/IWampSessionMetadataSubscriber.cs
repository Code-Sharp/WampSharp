using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSessionMetadataSubscriber
    {
        /// <summary>
        /// Fired when a session joins a realm on the router.
        /// </summary>
        /// <param name="details">An object describing the joined session.</param>
        [WampTopic("wamp.session.on_join")]
        void OnJoin(WampSessionDetails details);

        /// <summary>
        /// Fired when a session leaves a realm on the router or is disconnected.
        /// </summary>
        /// <param name="sessionId">The session ID of the session that left.</param>
        [WampTopic("wamp.session.on_leave")]
        void OnLeave(long sessionId);
    }
}