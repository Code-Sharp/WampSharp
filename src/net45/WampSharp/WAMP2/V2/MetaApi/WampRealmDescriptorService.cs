using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    public class WampRealmDescriptorService : 
        IWampSessionDescriptor,
        IWampSubscriptionDescriptor,
        IWampRegistrationDescriptor
    {
        private readonly IWampSessionDescriptor mSessionDescriptor;
        private readonly IWampSubscriptionDescriptor mSubscriptionDescriptor;
        private readonly IWampRegistrationDescriptor mRegistrationDescriptor;

        public WampRealmDescriptorService(IWampHostedRealm realm)
        {
            mSessionDescriptor = new SessionDescriptorService(realm);
            mSubscriptionDescriptor = new SubscriptionDescriptorService(realm);
            mRegistrationDescriptor = new RegistrationDescriptorService(realm);
        }

        public long SessionCount()
        {
            return mSessionDescriptor.SessionCount();
        }

        public long[] GetSessionIds()
        {
            return mSessionDescriptor.GetSessionIds();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {
            return mSessionDescriptor.GetSessionDetails(sessionId);
        }

        public AvailableGroups GetAllRegistrations()
        {
            return mRegistrationDescriptor.GetAllRegistrations();
        }

        public long LookupRegistrationId(string procedureUri, RegisterOptions options = null)
        {
            return mRegistrationDescriptor.LookupRegistrationId(procedureUri, options);
        }

        public long[] GetMatchingRegistrationIds(string procedureUri)
        {
            return mRegistrationDescriptor.GetMatchingRegistrationIds(procedureUri);
        }

        public RegistrationDetails GetRegistrationDetails(long registrationId)
        {
            return mRegistrationDescriptor.GetRegistrationDetails(registrationId);
        }

        public long[] GetCalleesIds(long registrationId)
        {
            return mRegistrationDescriptor.GetCalleesIds(registrationId);
        }

        public long CountCallees(long registrationId)
        {
            return mRegistrationDescriptor.CountCallees(registrationId);
        }

        public AvailableGroups GetAllSubscriptionIds()
        {
            return mSubscriptionDescriptor.GetAllSubscriptionIds();
        }

        public long LookupSubscriptionId(string topicUri, SubscribeOptions options = null)
        {
            return mSubscriptionDescriptor.LookupSubscriptionId(topicUri, options);
        }

        public long[] GetMatchingSubscriptionIds(string topicUri)
        {
            return mSubscriptionDescriptor.GetMatchingSubscriptionIds(topicUri);
        }

        public SubscriptionDetails GetSubscriptionDetails(long subscriptionId)
        {
            return mSubscriptionDescriptor.GetSubscriptionDetails(subscriptionId);
        }

        public long[] GetSubscribers(long subscriptionId)
        {
            return mSubscriptionDescriptor.GetSubscribers(subscriptionId);
        }

        public long CountSubscribers(long subscriptionId)
        {
            return mSubscriptionDescriptor.CountSubscribers(subscriptionId);
        }
    }
}