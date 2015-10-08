using System;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    internal class WampRealmDescriptorService : 
        IWampSessionDescriptor,
        IWampSubscriptionDescriptor,
        IWampRegistrationDescriptor,
        IDisposable
    {
        private readonly SessionDescriptorService mSessionDescriptor;
        private readonly SubscriptionDescriptorService mSubscriptionDescriptor;
        private readonly RegistrationDescriptorService mRegistrationDescriptor;

        public WampRealmDescriptorService(IWampHostedRealm realm)
        {
            mSessionDescriptor = new SessionDescriptorService(realm);
            mSubscriptionDescriptor = new SubscriptionDescriptorService(realm);
            mRegistrationDescriptor = new RegistrationDescriptorService(realm);
        }

        public void Dispose()
        {
            mSessionDescriptor.Dispose();
            mSubscriptionDescriptor.Dispose();
            mRegistrationDescriptor.Dispose();
        }

        public long CountSessions()
        {
            return mSessionDescriptor.CountSessions();
        }

        public long[] GetAllSessionIds()
        {
            return mSessionDescriptor.GetAllSessionIds();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {
            return mSessionDescriptor.GetSessionDetails(sessionId);
        }

        public AvailableGroups GetAllRegistrations()
        {
            return mRegistrationDescriptor.GetAllRegistrations();
        }

        public long? LookupRegistrationId(string procedureUri, RegisterOptions options = null)
        {
            return mRegistrationDescriptor.LookupRegistrationId(procedureUri, options);
        }

        public long? GetBestMatchingRegistrationId(string procedureUri)
        {
            return mRegistrationDescriptor.GetBestMatchingRegistrationId(procedureUri);
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

        public long? LookupSubscriptionId(string topicUri, SubscribeOptions options = null)
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