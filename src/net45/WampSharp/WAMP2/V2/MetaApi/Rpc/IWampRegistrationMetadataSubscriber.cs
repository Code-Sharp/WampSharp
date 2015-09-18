using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampRegistrationMetadataSubscriber
    {
        [WampTopic("wamp.registration.on_create")]
        void OnCreate(long sessionId, RegistrationDetails details);

        [WampTopic("wamp.registration.on_register")]
        void OnRegister(long sessionId, long registrationId);

        [WampTopic("wamp.registration.on_unregister")]
        void OnUnregister(long sessionId, long registrationId);

        [WampTopic("wamp.registration.on_delete")]
        void OnDelete(long sessionId, long registrationId);
    }
}