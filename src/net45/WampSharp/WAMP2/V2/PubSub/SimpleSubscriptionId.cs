namespace WampSharp.V2.PubSub
{
    public abstract class SimpleSubscriptionId : IWampCustomizedSubscriptionId
    {
        private readonly string mTopicUri;

        public SimpleSubscriptionId(string topicUri)
        {
            mTopicUri = topicUri;
        }

        protected bool Equals(SimpleSubscriptionId other)
        {
            return string.Equals(mTopicUri, other.mTopicUri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SimpleSubscriptionId) obj);
        }

        public override int GetHashCode()
        {
            return (mTopicUri != null ? mTopicUri.GetHashCode() : 0);
        }
    }
}