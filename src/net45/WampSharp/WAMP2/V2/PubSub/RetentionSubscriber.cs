using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class RetentionSubscriber : IWampRawTopicRouterSubscriber
    {
        private IImmutableStack<RetainedEvent> mRetainedEvents = ImmutableStack<RetainedEvent>.Empty;
        private readonly object mLock = new object();

        public RetentionSubscriber(IWampTopic topic)
        {
            topic.SubscriptionAdded += OnSubscriptionAdded;
        }

        private void OnSubscriptionAdded(object sender, WampSubscriptionAddEventArgs e)
        {
            if (e.Options.GetRetained == true)
            {
                foreach (RetainedEvent retainedEvent in mRetainedEvents)
                {
                    if (retainedEvent.Options.IsEligible(e.Subscriber))
                    {
                        retainedEvent.Publish(e.Subscriber);
                    }

                    break;
                }
            }
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options)
        {
            RetainEvent(options,
                        (subscriber, details) =>
                            subscriber.Event(publicationId, details));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options,
                                    TMessage[] arguments)
        {
            RetainEvent(options,
                        (subscriber, details) =>
                            subscriber.Event(publicationId,
                                             details,
                                             arguments.Cast<object>().ToArray()));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options,
                                    TMessage[] arguments,
                                    IDictionary<string, TMessage> argumentsKeywords)
        {
            RetainEvent(options,
                        (subscriber, details) =>
                            subscriber.Event(publicationId,
                                             details,
                                             arguments.Cast<object>().ToArray(),
                                             argumentsKeywords.ToDictionary(x => x.Key,
                                                                            x => (object) x.Value)));
        }

        private void RetainEvent(PublishOptions options,
                                 Action<IRemoteWampTopicSubscriber, EventDetails> action)
        {
            Array[] all = {
                options.ExcludeAuthenticationIds,
                options.ExcludeAuthenticationRoles,
                options.Exclude,
                options.EligibleAuthenticationIds,
                options.EligibleAuthenticationRoles,
                options.Eligible
            };

            RetainedEvent retainedEvent = new RetainedEvent(options, action);

            // If the event has no restrictions, then it is the most recent event.
            bool hasNoRestrictions = all.All(x => x == null);

            lock (mLock)
            {
                if (hasNoRestrictions)
                {
                    mRetainedEvents = ImmutableStack<RetainedEvent>.Empty;
                }

                mRetainedEvents = mRetainedEvents.Push(retainedEvent);
            }
        }

        private class RetainedEvent
        {
            public RetainedEvent(PublishOptions options,
                                 Action<IRemoteWampTopicSubscriber, EventDetails> action)
            {
                EventDetails eventDetails = options.GetEventDetails();
                eventDetails.Retained = true;
                Details = eventDetails;
                Options = options;
                Action = action;
            }

            private EventDetails Details { get; }

            public PublishOptions Options { get; }

            private Action<IRemoteWampTopicSubscriber, EventDetails> Action { get; }

            public void Publish(IRemoteWampTopicSubscriber subscriber)
            {
                Action(subscriber, Details);
            }
        }
    }
}