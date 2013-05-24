using System;
using WampSharp.Core.Contracts.V1;

namespace WampSharp.PubSub.Server
{
    public class WampObserver<TMessage> : IObserver<TMessage>
    {
        private readonly string mTopicUri;
        private readonly IWampClient mClient;

        public WampObserver(string topicUri, IWampClient client)
        {
            mTopicUri = topicUri;
            mClient = client;
        }

        public string SessionId
        {
            get
            {
                return mClient.SessionId;
            }
        }

        public void OnNext(TMessage value)
        {
            mClient.Event(mTopicUri, value);
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnCompleted()
        {
            //
        }

        protected bool Equals(WampObserver<TMessage> other)
        {
            return string.Equals(mTopicUri, other.mTopicUri) &&
                   Equals(mClient, other.mClient);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WampObserver<TMessage>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((mTopicUri != null ? mTopicUri.GetHashCode() : 0)*397) ^ (mClient != null ? mClient.GetHashCode() : 0);
            }
        }
    }
}