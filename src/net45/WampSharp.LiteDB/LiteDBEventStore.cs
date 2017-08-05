using System.Collections.Immutable;
using System.Linq;
using LiteDB;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.MetaApi;

namespace WampSharp.LiteDB
{
    public class LiteDBEventStore : IWampEventStore
    {
        private readonly LiteDatabase mDatabase;
        private readonly LiteCollection<SubscriptionEntry> mSubscriptions;
        private ImmutableDictionary<(string topic,string match), SubscriptionEntry> mLocalSubscriptions =
            ImmutableDictionary.Create<(string topic, string match), SubscriptionEntry>();

        private readonly object mLock = new object();

        private readonly LiteCollection<EventEntry> mEvents;

        public LiteDBEventStore(LiteDatabase database)
        {
            mDatabase = database;
            mSubscriptions = mDatabase.GetCollection<SubscriptionEntry>("subscriptions");
            mSubscriptions.EnsureIndex(x => x.Topic);
            mSubscriptions.EnsureIndex(x => x.Match);

            mEvents = mDatabase.GetCollection<EventEntry>("events");
            mEvents.EnsureIndex(x => x.Subscription);
        }

        public void StoreEvent<TMessage>(IWampFormatter<TMessage> formatter,
                                         HistoricalSubscription subscription,
                                         WampHistoricalEvent<TMessage> historicalEvent)
        {
            SubscriptionEntry storedSubscription = GetSubscription(subscription);

            EventEntry value = new EventEntry()
            {
                Subscription = storedSubscription,
                PublicationId = historicalEvent.PublicationId,
                PublisherId = historicalEvent.PublisherId,
                Timestamp = historicalEvent.Timestamp,
                Topic = historicalEvent.Topic,
                Arguments =
                    historicalEvent.Arguments.Select(x => JsonToBson.ConvertToBson
                                                         (formatter.Deserialize<JToken>(x)))
                                   .ToArray(),
                ArgumentsKeywords = historicalEvent.ArgumentsKeywords.ToDictionary(x => x.Key,
                                                                                   x => JsonToBson.ConvertToBson
                                                                                   (formatter.Deserialize<JToken>(
                                                                                        x.Value)))
            };

            mEvents.Insert(value);
        }

        private SubscriptionEntry GetSubscription(HistoricalSubscription subscription)
        {
            SubscriptionEntry value;

            if (mLocalSubscriptions.TryGetValue((subscription.TopicUri, subscription.SubscribeOptions.Match),
                                                out value))
            {
                return value;
            }

            SubscriptionEntry result;

            lock (mLock)
            {
                result =
                    mSubscriptions.FindOne(x => x.Match == subscription.SubscribeOptions.Match &&
                                                x.Topic == subscription.TopicUri);

                if (result == null)
                {
                    result = new SubscriptionEntry()
                    {
                        Match = subscription.SubscribeOptions.Match,
                        Topic = subscription.TopicUri
                    };

                    mSubscriptions.Insert(result);
                }


                mLocalSubscriptions = mLocalSubscriptions.SetItem((result.Topic, result.Match), result);
            }

            return result;
        }

        public WampHistoricalEvent<object>[] GetEvents(HistoricalSubscription subscription, long limit)
        {
            lock (mLock)
            {
                SubscriptionEntry subscriptionEntry = GetSubscription(subscription);
            }

            return null;
        }
    }
}
