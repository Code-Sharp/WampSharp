using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Curie;

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
            string resolvedUri = ResolveUri(client, topicUri);
            IWampTopic topic = GetTopicByUri(resolvedUri);
            topic.Subscribe(new WampObserver(topicUri, client));
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            string resolvedUri = ResolveUri(client, topicUri);
            IWampTopic topic = GetTopicByUri(resolvedUri);
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

            string resolvedUri = ResolveUri(client, topicUri);
            Publish(client, resolvedUri, @event, exclude);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude)
        {
            Publish(client, topicUri, @event, exclude, new string[] {});
        }

        private static string ResolveUri(IWampClient client, string topicUri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            return mapper.Resolve(topicUri);
        }

        #endregion
    }
}