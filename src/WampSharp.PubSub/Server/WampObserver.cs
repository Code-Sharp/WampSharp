using System;
using WampSharp.Core.Contracts.V1;

namespace WampSharp.PubSub.Server
{
    public class WampObserver : IObserver<object>
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
                return Client.SessionId;
            }
        }

        public IWampClient Client
        {
            get
            {
                return mClient;
            }
        }

        public void OnNext(object value)
        {
            Client.Event(mTopicUri, value);
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnCompleted()
        {
            //
        }

        protected bool Equals(WampObserver other)
        {
            return string.Equals(mTopicUri, other.mTopicUri) &&
                   Equals(Client, other.Client);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WampObserver) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((mTopicUri != null ? mTopicUri.GetHashCode() : 0)*397) ^ (Client != null ? Client.GetHashCode() : 0);
            }
        }
    }
}