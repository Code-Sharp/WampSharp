using System;
using System.Threading.Tasks;
using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public class RegistrationEvents : MetaApiEventsBase<IWampRegistrationMetadataSubscriber>
    {
        /// <summary>
        /// Fired when a registration is created through a registration 
        /// request for an URI which was previously without a registration.
        /// </summary>
        /// <param name="sessionId">The session ID performing the registration request.</param>
        /// <param name="details">Information on the created registration.</param>
        public delegate void OnCreateDelegate(long sessionId, RegistrationDetails details);

        /// <summary>
        /// Fired when a session is added to a registration.
        /// </summary>
        /// <param name="sessionId">The ID of the session being added to a registration.</param>
        /// <param name="registrationId">The ID of the registration to which a session is being added.</param>
        public delegate void OnRegisterDelegate(long sessionId, long registrationId);

        /// <summary>
        /// Fired when a session is removed from a subscription.
        /// </summary>
        /// <param name="sessionId">The ID of the session being removed from a registration.</param>
        /// <param name="registrationId">The ID of the registration from which a session is being removed.</param>
        public delegate void OnUnregisterDelegate(long sessionId, long registrationId);

        /// <summary>
        /// Fired when a registration is deleted after the last session attached to it has been removed.
        /// </summary>
        /// <param name="sessionId">The ID of the last session being removed from a registration.</param>
        /// <param name="registrationId">The ID of the registration being deleted.</param>
        public delegate void OnDeleteDelegate(long sessionId, long registrationId);

        public RegistrationEvents(IWampRealmProxy realmProxy) : base(realmProxy)
        {
        }

        public Task<IAsyncDisposable> OnCreate(OnCreateDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnCreate(default(long), default(RegistrationDetails)));
        }

        public Task<IAsyncDisposable> OnDelete(OnDeleteDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnDelete(default(long), default(long)));
        }

        public Task<IAsyncDisposable> OnRegister(OnRegisterDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnRegister(default(long), default(long)));
        }

        public Task<IAsyncDisposable> OnUnregister(OnUnregisterDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnUnregister(default(long), default(long)));
        }
    }
}