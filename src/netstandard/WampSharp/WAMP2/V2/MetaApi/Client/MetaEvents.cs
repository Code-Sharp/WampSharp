using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public class MetaEvents
    {
        private readonly RegistrationEvents mRegistrationEvents;

        public MetaEvents(IWampRealmProxy proxy)
        {
            Session = new SessionEvents(proxy);
            Subscription = new SubscriptionEvents(proxy);
            mRegistrationEvents = new RegistrationEvents(proxy);
        }

        public SessionEvents Session { get; }

        public SubscriptionEvents Subscription { get; }

        public RegistrationEvents Registration => mRegistrationEvents;
    }
}