using WampSharp.Core.Serialization;

namespace WampSharp.V2.Client
{
    public interface IWampRawTopicSubscriber
    {
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details);
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments);
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords);
    }
}