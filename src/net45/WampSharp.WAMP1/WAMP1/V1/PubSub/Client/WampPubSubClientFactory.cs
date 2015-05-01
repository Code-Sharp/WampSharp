using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.V1.PubSub.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampPubSubClientFactory{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampPubSubClientFactory<TMessage> : IWampPubSubClientFactory<TMessage>
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="WampPubSubClientFactory{TMessage}"/>.
        /// </summary>
        /// <param name="serverProxyFactory">The server proxy factory used to get callbacks.</param>
        /// <param name="formatter">The formatter used to serialize/deserialize messages.</param>
        public WampPubSubClientFactory(IWampServerProxyFactory<TMessage> serverProxyFactory,
                                       IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mServerProxyFactory = serverProxyFactory;
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri, IWampConnection<TMessage> connection)
        {
            return new WampPubSubSubject<TMessage, TEvent>(topicUri, mServerProxyFactory, connection, mFormatter);
        }
    }
}