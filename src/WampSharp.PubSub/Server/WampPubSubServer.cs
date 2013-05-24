using WampSharp.Core.Contracts.V1;

namespace WampSharp.PubSub.Server
{
    public class WampPubSubServer<TMessage> :
        IWampPubSubServer<TMessage>
    {
        private readonly WampTopicContainer<TMessage> mContainer =
            new WampTopicContainer<TMessage>();

        public void Subscribe(IWampClient client, string topicUri)
        {
            WampTopic<TMessage> topic = mContainer.GetTopicByUri(topicUri);
            topic.Subscribe(new WampObserver<TMessage>(topicUri, client));
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            WampTopic<TMessage> topic = mContainer.GetTopicByUri(topicUri);
            topic.Unsubscribe(client.SessionId);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            WampTopic<TMessage> topic = mContainer.GetTopicByUri(topicUri);
            topic.OnNext(new WampNotification<TMessage>(@event, exclude, eligible));
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
    }
}