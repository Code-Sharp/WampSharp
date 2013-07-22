using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.PubSub.Client
{
    public class WampPubSubClientFactory<TMessage> : IWampPubSubClientFactory<TMessage>
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

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