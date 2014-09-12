using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class RawRouterSubscriber : IWampRawTopicRouterSubscriber
    {
        private readonly IObserver<IWampSerializedEvent> mObserver;

        public RawRouterSubscriber(IObserver<IWampSerializedEvent> observer)
        {
            mObserver = observer;
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, GetEventDetails(options)));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, GetEventDetails(options), arguments));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mObserver.OnNext(new WampSerializedEvent<TMessage>(formatter, publicationId, GetEventDetails(options), arguments, argumentsKeywords));
        }

        private static EventDetails GetEventDetails(PublishOptions options)
        {
            EventDetails result = new EventDetails();

            PublishOptionsExtended extendedOptions = options as PublishOptionsExtended;

            if (extendedOptions != null)
            {
                result.Publisher = extendedOptions.PublisherId;
            }

            return result;
        }
    }
}