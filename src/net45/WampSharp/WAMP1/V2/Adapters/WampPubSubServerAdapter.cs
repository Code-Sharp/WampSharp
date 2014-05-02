using WampSharp.V1.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Adapters
{
    public class WampPubSubServerAdapter<TMessage> : V1.Core.Contracts.IWampPubSubServer<TMessage>
    {
        private readonly IWampTopicContainer mTopicContainer;

        public WampPubSubServerAdapter(IWampTopicContainer topicContainer)
        {
            mTopicContainer = topicContainer;
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
            throw new System.NotImplementedException();
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            throw new System.NotImplementedException();
        }
    }
}