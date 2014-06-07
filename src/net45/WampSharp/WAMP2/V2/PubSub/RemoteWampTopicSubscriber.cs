using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class RemoteWampTopicSubscriber : IRemoteWampTopicSubscriber
    {
        private readonly IWampClient mSubscriber;
        private readonly long mSubscriptionId;
        private readonly WampIdGenerator mIdGenerator = new WampIdGenerator();

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

        private long GeneratePublicationId()
        {
            return mIdGenerator.Generate();
        }

        public void Event(object details)
        {
            long publicationId = GeneratePublicationId();
            mSubscriber.Event(this.SubscriptionId, publicationId, details);
        }

        public void Event(object details, object[] arguments)
        {
            long publicationId = GeneratePublicationId();
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments);
        }

        public void Event(object details, object[] arguments, object argumentsKeywords)
        {
            long publicationId = GeneratePublicationId();
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments, argumentsKeywords);
        }
    }
}