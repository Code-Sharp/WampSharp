using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class SubscriptionDetailsExtended : SubscriptionDetails, IGroupDetailsExtended
    {
        private ImmutableList<long> mSubscribers = ImmutableList<long>.Empty;
        private readonly object mLock = new object();

        [IgnoreDataMember]
        public IReadOnlyList<long> Subscribers
        {
            get
            {
                return mSubscribers;
            }
        }

        public void AddSubscriber(long sessionId)
        {
            lock (mLock)
            {
                mSubscribers = mSubscribers.Add(sessionId);
            }
        }

        public void RemoveSubscriber(long sessionId)
        {
            lock (mLock)
            {
                mSubscribers = mSubscribers.Remove(sessionId);
            }
        }

        long IGroupDetailsExtended.GroupId
        {
            get
            {
                return SubscriptionId;
            }
        }

        IReadOnlyList<long> IGroupDetailsExtended.Peers
        {
            get
            {
                return Subscribers;
            }
        }

        void IGroupDetailsExtended.AddPeer(long sessionId)
        {
            AddSubscriber(sessionId);
        }

        void IGroupDetailsExtended.RemovePeer(long sessionId)
        {
            RemoveSubscriber(sessionId);
        }
    }
}