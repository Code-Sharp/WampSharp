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

        private void OnSessionCreated(object sender, WampSessionEventArgs e)
        {
            WampSessionDetails sessionDetails = new WampSessionDetails()
            {
                Realm = mRealm.Name,
                Session = e.SessionId,
                AuthMethod = "anonymous",
                TransportDetails = e.TransportDetails
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
            private readonly IWampTopic mOnJoinTopic;
            private readonly IWampTopic mOnLeaveTopic;
            private readonly PublishOptions mPublishOptions = new PublishOptions();

            public SessionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mOnJoinTopic = topicContainer.CreateTopicByUri("wamp.session.on_join", true);
                mOnLeaveTopic = topicContainer.CreateTopicByUri("wamp.session.on_leave", true);
            }

            public void OnJoin(WampSessionDetails details)
            {
                mOnJoinTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] {details});
            }

            public void OnLeave(long sessionId)
            {
                mOnLeaveTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId });
            }
        }
    }
}