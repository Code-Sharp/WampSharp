using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class RemoteWampTopicSubscriber : IWampTopicSubscriber
    {
        private readonly IWampClient mSubscriber;
        private readonly long mSubscriptionId;

        public RemoteWampTopicSubscriber(long subscriptionId, IWampSubscriber subscriber)
        {
            mSubscriber = subscriber as IWampClient;
            mSubscriptionId = subscriptionId;
        }

        public long Session
        {
            get
            {
                return mSubscriber.Session;
            }
        }

        public long SubscriptionId
        {
            get
            {
                return mSubscriptionId;
            }
        }

        public void Event(long publicationId, object details)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details);
        }

        public void Event(long publicationId, object details, object[] arguments)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments);
        }

        public void Event(long publicationId, object details, object[] arguments, object argumentsKeywords)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments, argumentsKeywords);
        }
    }
}