using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampEventHistoryProvider
    {
        [WampProcedure("wamp.subscription.get_events")]
        WampHistoricalEvent[] GetEvents(long subscriptionId, long limit = 10);
    }

    public class MonitoredTopics
    {
        public string TopicUri { get; set; }

        public SubscribeOptions SubscribeOptions { get; set; }
    }

    public interface IWampEventStore
    {
        void StoreEvent<TMessage>(IWampFormatter<TMessage> formatter, long subscriptionId, WampHistoricalEvent<TMessage> historicalEvent);

        WampHistoricalEvent<object>[] GetEvents(long subscriptionId, long limit);
    }

    class WampInMemoryEventStore : IWampEventStore
    {
        public void StoreEvent<TMessage>
            (IWampFormatter<TMessage> formatter,
             long subscriptionId,
             WampHistoricalEvent<TMessage> historicalEvent)
        {
        }

        public WampHistoricalEvent<object>[] GetEvents(long subscriptionId, long limit)
        {
            return new WampHistoricalEvent<object>[] {};
        }
    }

    public class WampEventHistoryProvider : IDisposable
    {
        private readonly IWampEventStore mStore;
        private readonly IDisposable mDisposable;

        public WampEventHistoryProvider(IWampTopicContainer container,
                                        IEnumerable<MonitoredTopics> topicsToRecord, IWampEventStore store)
        {
            mStore = store;

            List<IDisposable> disposables = new List<IDisposable>();

            foreach (MonitoredTopics topicToRecord in topicsToRecord)
            {
                EventHistorySubscriber subscriber = new EventHistorySubscriber(this);

                IWampRegistrationSubscriptionToken disposable =
                    container.Subscribe(subscriber,
                                        topicToRecord.TopicUri,
                                        topicToRecord.SubscribeOptions);

                subscriber.SubscriptionId = disposable.TokenId;

                disposables.Add(disposable);
            }

            mDisposable = new CompositeDisposable(disposables);
        }

        private void OnEvent<TMessage>(IWampFormatter<TMessage> formatter, long subscriptionId, WampHistoricalEvent<TMessage> wampHistoricalEvent)
        {
            mStore.StoreEvent(formatter, subscriptionId, wampHistoricalEvent);
        }

        private class EventHistorySubscriber : IWampRawTopicRouterSubscriber
        {
            private readonly WampEventHistoryProvider mParent;

            public EventHistorySubscriber(WampEventHistoryProvider parent)
            {
                mParent = parent;
            }

            public long SubscriptionId
            {
                get;
                set;
            }

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

            private void InnerEvent<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments = null, IDictionary<string, TMessage> argumentsKeywords = null)
            {
                PublishOptionsExtended optionsExtended = options as PublishOptionsExtended;

                mParent.OnEvent(formatter,
                                SubscriptionId,
                                new WampHistoricalEvent<TMessage>()
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

        [WampProcedure("wamp.subscription.get_events")]
        public WampHistoricalEvent<object>[] GetEvents(long subscriptionId, long limit = 10)
        {
            return mStore.GetEvents(subscriptionId, limit);
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}