using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    internal class RegistrationDescriptorService : IWampRegistrationDescriptor
    {
        public AvailableRegistrations GetAllRegistrations()
        {
            throw new System.NotImplementedException();
        }

        public long LookupRegistrationId(string procedureUri, RegisterOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public long[] GetMatchingRegistrationIds(string procedureUri)
        {
            throw new System.NotImplementedException();
        }

        public RegistrationDetails GetRegistrationDetails(long registrationId)
        {
            throw new System.NotImplementedException();
        }

        public long[] GetCalleesIds(long registrationId)
        {
            throw new System.NotImplementedException();
        }

        public long CountCallees(long registrationId)
        {
            throw new System.NotImplementedException();
        }

        private class RegistrationMetadataSubscriber : ManualSubscriber<IWampRegistrationMetadataSubscriber>, IWampRegistrationMetadataSubscriber
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

            public void OnDelete(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnDeleteTopicUri, sessionId, subscriptionId);
            }
        }
    }
}