using WampSharp.Core.Contracts.V1;

namespace WampSharp.PubSub.Server
{
    public class WampPubSubServer<TMessage> :
        IWampPubSubServer<TMessage>
    {
        #region Fields

        private readonly IWampTopicContainer mContainer;

        private bool mCreateTopicOnDemand = true;

        public WampPubSubServer()
            : this(new WampTopicContainer<TMessage>())
        {
        }

        public WampPubSubServer(IWampTopicContainer container)
        {
            mContainer = container;
        }

        #endregion

        #region Private Methods

        private IWampTopic GetTopicByUri(string topicUri)
        {
            if (!CreateTopicOnDemand)
            {
                return mContainer.GetTopicByUri(topicUri);
            }
            else
            {
                return mContainer.GetOrCreateTopicByUri(topicUri, false);
            }
        }

        #endregion

        #region Properties

        public bool CreateTopicOnDemand
        {
            get
            {
                return mCreateTopicOnDemand;
            }
            set
            {
                mCreateTopicOnDemand = value;
            }
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mContainer;
            }
        }

        #endregion

        #region IWampPubSubServer Methods

        public void Subscribe(IWampClient client, string topicUri)
        {
            IWampTopic topic = GetTopicByUri(topicUri);
            topic.Subscribe(new WampObserver(topicUri, client));
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            IWampTopic topic = GetTopicByUri(topicUri);
            topic.Unsubscribe(client.SessionId);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            IWampTopic topic = GetTopicByUri(topicUri);
            topic.OnNext(new WampNotification(@event, exclude, eligible));
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event)
        {
            Publish(client, topicUri, @event, true);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe)
        {
            string[] exclude;

            if (!excludeMe)
            {
                exclude = new string[0];
            }
            else
            {
                exclude = new[] {client.SessionId};
            }

            Publish(client, topicUri, @event, exclude);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude)
        {
            Publish(client, topicUri, @event, exclude, new string[] {});
        }

        #endregion
    }
}