using System.Threading.Tasks;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    public class WampMetaApiServiceProxy : IWampRegistrationDescriptorProxy, IWampSessionDescriptorProxy, IWampSubscriptionDescriptorProxy
    {
        private readonly WampRegistrationDescriptorProxyProxy mRegistrationDescriptorProxy;
        private readonly WampSessionDescriptorProxyProxy mSessionDescriptorProxy;
        private readonly WampSubscriptionDescriptorProxyProxy mSubscriptionDescriptor;

        internal WampMetaApiServiceProxy(IWampRealmProxy proxy)
        {
            ICalleeProxyInterceptor calleeProxyInterceptor = CalleeProxyInterceptor.Default;

            mRegistrationDescriptorProxy = new WampRegistrationDescriptorProxyProxy(proxy, calleeProxyInterceptor);
            mSessionDescriptorProxy = new WampSessionDescriptorProxyProxy(proxy, calleeProxyInterceptor);
            mSubscriptionDescriptor = new WampSubscriptionDescriptorProxyProxy(proxy, calleeProxyInterceptor);
            SubscribeTo = new MetaEvents(proxy);
        }

        public MetaEvents SubscribeTo { get; }

        public Task<long> CountSessionsAsync()
        {
            return mSessionDescriptorProxy.CountSessionsAsync();
        }

        public Task<long[]> GetAllSessionIdsAsync()
        {
            return mSessionDescriptorProxy.GetAllSessionIdsAsync();
        }

        public Task<WampSessionDetails> GetSessionDetailsAsync(long sessionId)
        {
            return mSessionDescriptorProxy.GetSessionDetailsAsync(sessionId);
        }

        public long CountSessions()
        {
            return mSessionDescriptorProxy.CountSessions();
        }

        public long[] GetAllSessionIds()
        {
            return mSessionDescriptorProxy.GetAllSessionIds();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {
            return mSessionDescriptorProxy.GetSessionDetails(sessionId);
        }

        public Task<AvailableGroups> GetAllRegistrationsAsync()
        {
            return mRegistrationDescriptorProxy.GetAllRegistrationsAsync();
        }

        public Task<long?> LookupRegistrationIdAsync(string procedureUri, RegisterOptions options)
        {
            return mRegistrationDescriptorProxy.LookupRegistrationIdAsync(procedureUri, options);
        }

        public Task<long?> GetBestMatchingRegistrationIdAsync(string procedureUri)
        {
            return mRegistrationDescriptorProxy.GetBestMatchingRegistrationIdAsync(procedureUri);
        }

        public Task<RegistrationDetails> GetRegistrationDetailsAsync(long registrationId)
        {
            return mRegistrationDescriptorProxy.GetRegistrationDetailsAsync(registrationId);
        }

        public Task<long[]> GetCalleesIdsAsync(long registrationId)
        {
            return mRegistrationDescriptorProxy.GetCalleesIdsAsync(registrationId);
        }

        public Task<long> CountCalleesAsync(long registrationId)
        {
            return mRegistrationDescriptorProxy.CountCalleesAsync(registrationId);
        }

        public AvailableGroups GetAllRegistrations()
        {
            return mRegistrationDescriptorProxy.GetAllRegistrations();
        }

        public long? LookupRegistrationId(string procedureUri, RegisterOptions options)
        {
            return mRegistrationDescriptorProxy.LookupRegistrationId(procedureUri, options);
        }

        public long? GetBestMatchingRegistrationId(string procedureUri)
        {
            return mRegistrationDescriptorProxy.GetBestMatchingRegistrationId(procedureUri);
        }

        public RegistrationDetails GetRegistrationDetails(long registrationId)
        {
            return mRegistrationDescriptorProxy.GetRegistrationDetails(registrationId);
        }

        public long[] GetCalleesIds(long registrationId)
        {
            return mRegistrationDescriptorProxy.GetCalleesIds(registrationId);
        }

        public long CountCallees(long registrationId)
        {
            return mRegistrationDescriptorProxy.CountCallees(registrationId);
        }

        public AvailableGroups GetAllSubscriptionIds()
        {
            return mSubscriptionDescriptor.GetAllSubscriptionIds();
        }

        public long? LookupSubscriptionId(string topicUri, SubscribeOptions options)
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

        public Task<AvailableGroups> GetAllSubscriptionIdsAsync()
        {
            return mSubscriptionDescriptor.GetAllSubscriptionIdsAsync();
        }

        public Task<long?> LookupSubscriptionIdAsync(string topicUri, SubscribeOptions options)
        {
            return mSubscriptionDescriptor.LookupSubscriptionIdAsync(topicUri, options);
        }

        public Task<long[]> GetMatchingSubscriptionIdsAsync(string topicUri)
        {
            return mSubscriptionDescriptor.GetMatchingSubscriptionIdsAsync(topicUri);
        }

        public Task<SubscriptionDetails> GetSubscriptionDetailsAsync(long subscriptionId)
        {
            return mSubscriptionDescriptor.GetSubscriptionDetailsAsync(subscriptionId);
        }

        public Task<long[]> GetSubscribersAsync(long subscriptionId)
        {
            return mSubscriptionDescriptor.GetSubscribersAsync(subscriptionId);
        }

        public Task<long> CountSubscribersAsync(long subscriptionId)
        {
            return mSubscriptionDescriptor.CountSubscribersAsync(subscriptionId);
        }
    }
}