using System;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    internal class SessionDescriptorService : IWampSessionDescriptor, IDisposable
    {
        private readonly IWampHostedRealm mRealm;
        private readonly IWampSessionMetadataSubscriber mSubscriber;

        private ImmutableDictionary<long, WampSessionDetails> mSessionIdToDetails =
            ImmutableDictionary<long, WampSessionDetails>.Empty; 

        public SessionDescriptorService(IWampHostedRealm realm)
        {
            mRealm = realm;
            mSubscriber = new SessionMetadataSubscriber(realm.TopicContainer);

            mRealm.SessionClosed += OnSessionClosed;
            mRealm.SessionCreated += OnSessionCreated;
        }

        private void OnSessionCreated(object sender, WampSessionCreatedEventArgs e)
        {
            WelcomeDetails welcomeDetails = e.WelcomeDetails;

            WampSessionDetails sessionDetails = new WampSessionDetails()
            {
                Realm = mRealm.Name,
                Session = e.SessionId,
                AuthMethod = welcomeDetails.AuthenticationMethod ?? "anonymous",
                AuthId = welcomeDetails.AuthenticationId,
                AuthProvider = welcomeDetails.AuthenticationProvider,
                AuthRole = welcomeDetails.AuthenticationRole,
                TransportDetails = e.HelloDetails.TransportDetails
            };

            ImmutableInterlocked.TryAdd(ref mSessionIdToDetails, e.SessionId, sessionDetails);

            mSubscriber.OnJoin(sessionDetails);
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {

            ImmutableInterlocked.TryRemove(ref mSessionIdToDetails, e.SessionId, out WampSessionDetails sessionDetails);

            mSubscriber.OnLeave(e.SessionId);
        }

        public long CountSessions()
        {
            return mSessionIdToDetails.Count;
        }

        public long[] GetAllSessionIds()
        {
            return mSessionIdToDetails.Keys.ToArray();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {

            if (mSessionIdToDetails.TryGetValue(sessionId, out WampSessionDetails result))
            {
                return result;
            }

            throw new WampException(WampErrors.NoSuchSession);
        }

        public void Dispose()
        {
            mRealm.SessionClosed -= OnSessionClosed;
            mRealm.SessionCreated -= OnSessionCreated;
        }

        private class SessionMetadataSubscriber : ManualSubscriber<IWampSessionMetadataSubscriber>, IWampSessionMetadataSubscriber
        {
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private static readonly string mOnJoinUri = GetTopicUri(subscriber => subscriber.OnJoin(default(WampSessionDetails)));
            private static readonly string mOnLeaveUri = GetTopicUri(subscriber => subscriber.OnLeave(default(long)));

            public SessionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mTopicContainer = topicContainer;
                mTopicContainer.CreateTopicByUri(mOnJoinUri, true);
                mTopicContainer.CreateTopicByUri(mOnLeaveUri, true);
            }

            public void OnJoin(WampSessionDetails details)
            {
                mTopicContainer.Publish(mPublishOptions, mOnJoinUri, details);
            }

            public void OnLeave(long sessionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnLeaveUri, sessionId);
            }
        }
    }
}