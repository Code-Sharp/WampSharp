using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive.Disposables;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public class WampEventHistoryProvider : IDisposable
    {
        private readonly IWampEventStore mStore;
        private readonly IDisposable mDisposable;
        private readonly IImmutableDictionary<long, HistoricalSubscription> mSubscriptionIdToSubscription;

        public WampEventHistoryProvider(IWampTopicContainer container,
                                        IEnumerable<HistoricalSubscription> topicsToRecord, IWampEventStore store)
        {
            mStore = store;

            Dictionary<long, HistoricalSubscription> subscriptionIdToSubscription = 
                new Dictionary<long, HistoricalSubscription>();

            List<IDisposable> disposables = new List<IDisposable>();

            foreach (HistoricalSubscription topicToRecord in topicsToRecord)
            {
                EventHistorySubscriber subscriber = new EventHistorySubscriber(this);

                IWampRegistrationSubscriptionToken disposable =
                    container.Subscribe(subscriber,
                                        topicToRecord.TopicUri,
                                        topicToRecord.SubscribeOptions);

                long subscriptionId = disposable.TokenId;

                HistoricalSubscription historicalSubscription =
                    new HistoricalSubscription
                    {
                        SubscribeOptions = topicToRecord.SubscribeOptions,
                        TopicUri = topicToRecord.TopicUri,
                        SubscriptionId = subscriptionId
                    };

                subscriber.Subscription = historicalSubscription;

                subscriptionIdToSubscription[subscriptionId] = historicalSubscription;

                disposables.Add(disposable);
            }

            mSubscriptionIdToSubscription = 
                subscriptionIdToSubscription.ToImmutableDictionary();

            mDisposable = new CompositeDisposable(disposables);
        }

        private class EventHistorySubscriber : IWampRawTopicRouterSubscriber
        {
            private readonly WampEventHistoryProvider mParent;

            public EventHistorySubscriber(WampEventHistoryProvider parent)
            {
                mParent = parent;
            }

            public HistoricalSubscription Subscription { get; set; }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter,
                                        long publicationId,
                                        PublishOptions options)
            {
                InnerEvent(formatter, publicationId, options);
            }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter,
                                        long publicationId,
                                        PublishOptions options,
                                        TMessage[] arguments)
            {
                InnerEvent(formatter, publicationId, options, arguments);
            }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter,
                                        long publicationId,
                                        PublishOptions options,
                                        TMessage[] arguments,
                                        IDictionary<string, TMessage> argumentsKeywords)
            {
                InnerEvent(formatter, publicationId, options, arguments, argumentsKeywords);
            }

            private void InnerEvent<TMessage>(IWampFormatter<TMessage> formatter, long publicationId,
                                              PublishOptions options, TMessage[] arguments = null,
                                              IDictionary<string, TMessage> argumentsKeywords = null)
            {
                PublishOptionsExtended optionsExtended = options as PublishOptionsExtended;

                mParent.OnEvent(formatter,
                                Subscription,
                                new WampHistoricalEvent<TMessage>
                                {
                                    Timestamp = DateTime.Now,
                                    Topic = optionsExtended.TopicUri,
                                    Arguments = arguments,
                                    ArgumentsKeywords = argumentsKeywords,
                                    PublicationId = publicationId,
                                    PublisherId = optionsExtended.PublisherId
                                });
            }

        }

        private void OnEvent<TMessage>(IWampFormatter<TMessage> formatter,
                                       HistoricalSubscription subscription,
                                       WampHistoricalEvent<TMessage> historicalEvent)
        {
            mStore.StoreEvent(formatter, subscription, historicalEvent);
        }

        [WampProcedure("wamp.subscription.get_events")]
        public WampHistoricalEvent<object>[] GetEvents(long subscriptionId, long limit = 10)
        {
            HistoricalSubscription subscription;

            if (mSubscriptionIdToSubscription.TryGetValue(subscriptionId, out subscription))
            {
                return mStore.GetEvents(subscription, limit);
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}