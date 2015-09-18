using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSessionMetadataSubscriber
    {
        [WampTopic("wamp.session.on_join")]
        void OnJoin(WampSessionDetails details);

        [WampTopic("wamp.session.on_leave")]
        void OnLeave(long sessionId);
    }
}