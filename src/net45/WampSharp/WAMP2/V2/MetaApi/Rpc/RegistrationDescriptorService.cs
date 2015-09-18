using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    internal class RegistrationDescriptorService : 
        DescriptorServiceBase<RegistrationDetailsExtended>,
        IWampRegistrationDescriptor
    {
        public RegistrationDescriptorService(IWampRealm realm) : 
            base(new RegistrationMetadataSubscriber(realm.TopicContainer), WampErrors.NoSuchRegistration)
        {
        }

        public AvailableGroups GetAllRegistrations()
        {
            return GetAllGroupIds();
        }

        public long LookupRegistrationId(string procedureUri, RegisterOptions options = null)
        {
            string match = null;

            if (options != null)
            {
                match = options.Match;
            }

            return base.LookupGroupId(procedureUri, match);
        }

        public long[] GetMatchingRegistrationIds(string procedureUri)
        {
            return base.GetMatchingGroupIds(procedureUri);
        }

        public RegistrationDetails GetRegistrationDetails(long registrationId)
        {
            return base.GetGroupDetails(registrationId);
        }

        public long[] GetCalleesIds(long registrationId)
        {
            return base.GetPeersIds(registrationId);
        }

        public long CountCallees(long registrationId)
        {
            return base.CountPeers(registrationId);
        }

        private class RegistrationMetadataSubscriber : 
            ManualSubscriber<IWampRegistrationMetadataSubscriber>, 
            IWampRegistrationMetadataSubscriber,
            IDescriptorSubscriber<RegistrationDetailsExtended>
        {
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private static readonly string mOnCreateTopicUri = GetTopicUri(subscriber => subscriber.OnCreate(default(long), default(RegistrationDetails)));
            private static readonly string mOnRegisterTopicUri = GetTopicUri(subscriber => subscriber.OnRegister(default(long), default(long)));
            private static readonly string mOnUnregisterTopicUri = GetTopicUri(subscriber => subscriber.OnUnregister(default(long), default(long)));
            private static readonly string mOnDeleteTopicUri = GetTopicUri(subscriber => subscriber.OnDelete(default(long), default(long)));

            public RegistrationMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mTopicContainer = topicContainer;
                mTopicContainer.CreateTopicByUri(mOnCreateTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnRegisterTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnUnregisterTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnDeleteTopicUri, true);
            }

            public void OnCreate(long sessionId, RegistrationDetails details)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, details);
            }

            public void OnRegister(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnRegisterTopicUri, sessionId, registrationId);
            }

            public void OnUnregister(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnUnregisterTopicUri, sessionId, registrationId);
            }

            public void OnDelete(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnDeleteTopicUri, sessionId, registrationId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnCreate(long sessionId, RegistrationDetailsExtended details)
            {
                OnCreate(sessionId, details);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnJoin(long sessionId, long groupId)
            {
                OnRegister(sessionId, groupId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnLeave(long sessionId, long groupId)
            {
                OnUnregister(sessionId, groupId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnDelete(long sessionId, long groupId)
            {
                OnDelete(sessionId, groupId);
            }
        }
    }
}