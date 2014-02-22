using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class RemoteWampTopicSubscriber : IWampTopicSubscriber
    {
        private readonly IWampSubscriber mSubscriber;

        public RemoteWampTopicSubscriber(IWampSubscriber subscriber)
        {
            mSubscriber = subscriber;
        }

        public long SubscriptionId
        {
            get; 
            // TODO: I'm Not happy with this setter
            set;
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