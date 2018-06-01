using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Curie;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// An implementation of <see cref="WampPubSubServer{TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampPubSubServer<TMessage> :
        IWampPubSubServer<TMessage>
    {
        #region Fields

        private readonly IWampTopicContainerExtended<TMessage> mContainer;

        /// <summary>
        /// Initializes a new instance of <see cref="WampPubSubServer{TMessage}"/>.
        /// </summary>
        public WampPubSubServer()
            : this(new WampTopicContainer<TMessage>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampPubSubServer{TMessage}"/>.
        /// </summary>
        /// <param name="container">The <see cref="IWampTopicContainerExtended{TMessage}"/>
        /// the server will work against.</param>
        public WampPubSubServer(IWampTopicContainerExtended<TMessage> container)
        {
            mContainer = container;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="IWampTopicContainer"/> this server works against.
        /// </summary>
        public IWampTopicContainer TopicContainer => mContainer;

        #endregion

        #region IWampPubSubServer Methods

        public void Subscribe(IWampClient client, string topicUri)
        {
            string resolvedUri = ResolveUri(client, topicUri);
            mContainer.Subscribe(resolvedUri, new WampObserver(topicUri, client));
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            string resolvedUri = ResolveUri(client, topicUri);
            mContainer.Unsubscribe(resolvedUri, client.SessionId);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            string resolvedUri = ResolveUri(client, topicUri);
            mContainer.Publish(resolvedUri, @event, exclude, eligible);
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