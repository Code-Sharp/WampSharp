using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampRegistrationMetadataSubscriber
    {
        /// <summary>
        /// Fired when a registration is created through a registration 
        /// request for an URI which was previously without a registration.
        /// </summary>
        /// <param name="sessionId">The session ID performing the registration request.</param>
        /// <param name="details">Information on the created registration.</param>
        [WampTopic("wamp.registration.on_create")]
        void OnCreate(long sessionId, RegistrationDetails details);

        /// <summary>
        /// Fired when a session is added to a registration.
        /// </summary>
        /// <param name="sessionId">The ID of the session being added to a registration.</param>
        /// <param name="registrationId">The ID of the registration to which a session is being added.</param>
        [WampTopic("wamp.registration.on_register")]
        void OnRegister(long sessionId, long registrationId);

        /// <summary>
        /// Fired when a session is removed from a subscription.
        /// </summary>
        /// <param name="sessionId">The ID of the session being removed from a registration.</param>
        /// <param name="registrationId">The ID of the registration from which a session is being removed.</param>
        [WampTopic("wamp.registration.on_unregister")]
        void OnUnregister(long sessionId, long registrationId);

        /// <summary>
        /// Fired when a registration is deleted after the last session attached to it has been removed.
        /// </summary>
        /// <param name="sessionId">The ID of the last session being removed from a registration.</param>
        /// <param name="registrationId">The ID of the registration being deleted.</param>
        [WampTopic("wamp.registration.on_delete")]
        void OnDelete(long sessionId, long registrationId);
    }
}