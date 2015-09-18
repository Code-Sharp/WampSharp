using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    public class SessionDescriptorService : IWampSessionDescriptor
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
            WampSessionDetails sessionDetails = new WampSessionDetails()
            {
                Realm = mRealm.Name,
                Session = e.SessionId,
                AuthMethod = e.WelcomeDetails.AuthenticationMethod ?? "anonymous",
                TransportDetails = e.HelloDetails.TransportDetails
            };

            ImmutableInterlocked.TryAdd(ref mSessionIdToDetails, e.SessionId, sessionDetails);

            mSubscriber.OnJoin(sessionDetails);
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {
            WampSessionDetails sessionDetails;

            ImmutableInterlocked.TryRemove(ref mSessionIdToDetails, e.SessionId, out sessionDetails);

            mSubscriber.OnLeave(e.SessionId);
        }

        public long SessionCount()
        {
            return mSessionIdToDetails.Count;
        }

        public long[] GetSessionIds()
        {
            return mSessionIdToDetails.Keys.ToArray();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {
            WampSessionDetails result;

            if (mSessionIdToDetails.TryGetValue(sessionId, out result))
            {
                return result;
            }

            throw new WampException(WampErrors.NoSuchSession);
        }

        private class SessionMetadataSubscriber : IWampSessionMetadataSubscriber
        {
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private string mOnJoinUri = "wamp.session.on_join";
            private string mOnLeaveUri = "wamp.session.on_leave";

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