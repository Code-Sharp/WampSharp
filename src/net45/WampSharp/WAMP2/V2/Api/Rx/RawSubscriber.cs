using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class RawSubscriber : IWampRawTopicSubscriber
    {
        private readonly IObserver<IWampSerializedEvent> mObserver;

        public RawSubscriber(IObserver<IWampSerializedEvent> observer)
        {
            mObserver = observer;
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, details));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, details, arguments));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, details, arguments, argumentsKeywords));
        }
    }
}