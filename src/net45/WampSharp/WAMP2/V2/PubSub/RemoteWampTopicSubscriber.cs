using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class RemoteWampTopicSubscriber : IRemoteWampTopicSubscriber
    {
        private readonly IWampClientProxy mSubscriber;

        public RemoteWampTopicSubscriber(long subscriptionId, IWampSubscriber subscriber)
        {
            mSubscriber = subscriber as IWampClientProxy;
            SubscriptionId = subscriptionId;
        }

        public long SessionId
        {
            get
            {
                return mSubscriber.Session;
            }
        }

        public string AuthenticationId
        {
            get
            {
                return WelcomeDetails?.AuthenticationId;
            }
        }

        public string AuthenticationRole
        {
            get
            {
                return WelcomeDetails?.AuthenticationRole;
            }
        }

        private WelcomeDetails WelcomeDetails
        {
            get
            {
                return mSubscriber.WelcomeDetails;
            }
        }

        public long SubscriptionId { get; }

        public void Event(long publicationId, EventDetails details)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details);
        }

        public void Event(long publicationId, EventDetails details, object[] arguments)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments);
        }

        public void Event(long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mSubscriber.Event(this.SubscriptionId, publicationId, details, arguments, argumentsKeywords);
        }
    }
}