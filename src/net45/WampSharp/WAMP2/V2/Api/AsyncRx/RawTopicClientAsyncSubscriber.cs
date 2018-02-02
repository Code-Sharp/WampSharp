using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class RawTopicClientAsyncSubscriber : IWampRawTopicClientSubscriber
    {
        private readonly IAsyncObserver<IWampSerializedEvent> mObserver;

        public RawTopicClientAsyncSubscriber(IAsyncObserver<IWampSerializedEvent> observer)
        {
            mObserver = observer;
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details)
        {
            mObserver.OnNextAsync(new WampSerializedEvent<TMessage>(formatter, publicationId, details));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments)
        {
            mObserver.OnNextAsync(new WampSerializedEvent<TMessage>(formatter, publicationId, details, arguments));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mObserver.OnNextAsync(new WampSerializedEvent<TMessage>(formatter, publicationId, details, arguments, argumentsKeywords));
        }
    }
}