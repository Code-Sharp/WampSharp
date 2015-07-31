using System.Collections.Concurrent;
using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Reflection
{
    public class SessionDescriptorService : IWampSessionDescriptor
    {
        private readonly IWampHostedRealm mRealm;
        private readonly IWampSessionMetadataSubscriber mSubscriber;

        private readonly ConcurrentDictionary<long, WampSessionDetails> mSessionIdToDetails =
            new ConcurrentDictionary<long, WampSessionDetails>(); 

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
            
            mSessionIdToDetails[e.SessionId] = sessionDetails;

            mSubscriber.OnJoin(sessionDetails);
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {
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

            throw new WampException("wamp.error.no_such_session");
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