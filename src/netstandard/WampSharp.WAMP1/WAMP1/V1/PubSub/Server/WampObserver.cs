using System;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents a proxy to a topic's subscriber.
    /// </summary>
    public class WampObserver : IObserver<object>
    {
        private readonly string mTopicUri;

        /// <summary>
        /// Initializes a new instance of <see cref="WampObserver"/>.
        /// </summary>
        /// <param name="topicUri">The uri of the topic the observer belongs to.</param>
        /// <param name="client">The proxy to the client.</param>
        public WampObserver(string topicUri, IWampClient client)
        {
            mTopicUri = topicUri;
            Client = client;
        }

        /// <summary>
        /// Gets the session id of the client.
        /// </summary>
        public string SessionId => Client.SessionId;

        /// <summary>
        /// Gets a proxy to the client.
        /// </summary>
        public IWampClient Client { get; }

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