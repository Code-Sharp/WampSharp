using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IWampRawTopicRouterSubscriber
    {
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options);
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments);
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments, TMessage argumentsKeywords);
    }
}