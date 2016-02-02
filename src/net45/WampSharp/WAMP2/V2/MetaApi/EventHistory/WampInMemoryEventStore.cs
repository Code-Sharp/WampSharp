using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class WampInMemoryEventStore : IWampEventStore
    {
        private readonly ConcurrentDictionary<long, WampHistoricalEvent<object>> mPublicationIdToPublication =
            new ConcurrentDictionary<long, WampHistoricalEvent<object>>(); 

        public void StoreEvent<TMessage>(IWampFormatter<TMessage> formatter,
                                         HistoricalSubscription subscription,
                                         WampHistoricalEvent<TMessage> historicalEvent)
        {
            mPublicationIdToPublication.GetOrAdd(historicalEvent.PublicationId, x => ConvertEvent(historicalEvent));

            // TODO: add this to subscriptionId to publicationId data structure.
        }

        private static WampHistoricalEvent<object> ConvertEvent<TMessage>(WampHistoricalEvent<TMessage> historicalEvent)
        {
            object[] arguments;

            if (historicalEvent.Arguments == null)
            {
                arguments = null;
            }
            else
            {
                arguments = historicalEvent.Arguments.Cast<object>().ToArray();
            }

            IDictionary<string, object> argumentsKeywords;

            if (historicalEvent.ArgumentsKeywords == null)
            {
                argumentsKeywords = null;
            }
            else
            {
                argumentsKeywords =
                    historicalEvent.ArgumentsKeywords
                                   .ToDictionary(x => x.Key,
                                                 x => (object) x.Value);
            }

            WampHistoricalEvent<object> boxed = new WampHistoricalEvent<object>()
            {
                Arguments = arguments,
                ArgumentsKeywords = argumentsKeywords,
                PublicationId = historicalEvent.PublicationId,
                PublisherId = historicalEvent.PublisherId,
                Timestamp = historicalEvent.Timestamp,
                Topic = historicalEvent.Topic
            };

            return boxed;
        }

        public WampHistoricalEvent<object>[] GetEvents(HistoricalSubscription subscription, long limit)
        {
            return new WampHistoricalEvent<object>[] {};
        }
    }
}