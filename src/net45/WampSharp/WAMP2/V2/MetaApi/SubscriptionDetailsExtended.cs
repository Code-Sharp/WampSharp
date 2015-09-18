using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class SubscriptionDetailsExtended : SubscriptionDetails
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
    }
}