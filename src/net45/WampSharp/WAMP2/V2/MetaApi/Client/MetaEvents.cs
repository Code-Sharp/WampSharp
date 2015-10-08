using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public class MetaEvents
    {
        private readonly SessionEvents mSessionEvents;
        private readonly SubscriptionEvents mSubscriptionEvents;
        private readonly RegistrationEvents mRegistrationEvents;

        public MetaEvents(IWampRealmProxy proxy)
        {
            mSessionEvents = new SessionEvents(proxy);
            mSubscriptionEvents = new SubscriptionEvents(proxy);
            mRegistrationEvents = new RegistrationEvents(proxy);
        }

        public SessionEvents Session
        {
            get
            {
                return mSessionEvents;
            }
        }

        public SubscriptionEvents Subscription
        {
            get
            {
                return mSubscriptionEvents;
            }
        }

        public RegistrationEvents Registration
        {
            get
            {
                return mRegistrationEvents;
            }
        }
    }
}