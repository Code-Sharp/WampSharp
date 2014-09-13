namespace WampSharp.V2.PubSub
{
    internal class OptionLessSubscriptionId : IWampCustomizedSubscriptionId
    {
        private readonly string mTopicUri;

        public OptionLessSubscriptionId(string topicUri)
        {
            mTopicUri = topicUri;
        }

        protected bool Equals(OptionLessSubscriptionId other)
        {
            return string.Equals(mTopicUri, other.mTopicUri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OptionLessSubscriptionId) obj);
        }

        public override int GetHashCode()
        {
            return (mTopicUri != null ? mTopicUri.GetHashCode() : 0);
        }
    }
}