using System.Reactive.Subjects;
using WampSharp.Core.Serialization;

namespace WampSharp.PubSub
{
    public class WampPubSubClientFactory<TMessage> : IWampPubSubClientFactory
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampPubSubClientFactory(IWampServerProxyFactory<TMessage> serverProxyFactory,
                                       IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mServerProxyFactory = serverProxyFactory;
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            return new WampPubSubSubject<TMessage, TEvent>(topicUri, mFormatter, mServerProxyFactory);
        }
    }
}