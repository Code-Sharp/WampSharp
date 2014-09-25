namespace WampSharp.V2.PubSub
{
    public class OptionlessSubscriptionId : IWampCustomizedSubscriptionId
    {
        private readonly string mTopicUri;

        public OptionlessSubscriptionId(string topicUri)
        {
            mTopicUri = topicUri;
        }

        protected bool Equals(OptionlessSubscriptionId other)
        {
            return string.Equals(mTopicUri, other.mTopicUri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OptionlessSubscriptionId) obj);
        }

        public override int GetHashCode()
        {
            return (mTopicUri != null ? mTopicUri.GetHashCode() : 0);
        }
    }
}