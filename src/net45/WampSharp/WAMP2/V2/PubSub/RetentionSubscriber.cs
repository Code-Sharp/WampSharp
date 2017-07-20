using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class RetentionSubscriber : IWampRawTopicWeakRouterSubscriber
    {
        private readonly SubscribeOptions mOptions;

        private IImmutableStack<RetainedEvent> mRetainedEvents = ImmutableStack<RetainedEvent>.Empty;

        public RetentionSubscriber(SubscribeOptions options, IWampTopic topic)
        {
            mOptions = options;
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

        private string Match
        {
            get
            {
                return this.mOptions.Match;
            }
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

            RetainedEvent retainedEvent = new RetainedEvent(options, Match, action);

            // If the event has no restrictions, then it is the most recent event.
            if (all.All(x => (x == null) || (x.Length == 0)))
            {
                mRetainedEvents = ImmutableStack<RetainedEvent>.Empty;
            }

            mRetainedEvents.Push(retainedEvent);
        }

        private class RetainedEvent
        {
            public RetainedEvent(PublishOptions options,
                                 string match,
                                 Action<IRemoteWampTopicSubscriber, EventDetails> action)
            {
                EventDetails eventDetails = options.GetEventDetails(Match);
                eventDetails.Retained = true;
                Details = eventDetails;
                Options = options;
                Match = match;
                Action = action;
            }

            private EventDetails Details { get; set; }

            public PublishOptions Options { get; private set; }

            private string Match { get; set; }

            private Action<IRemoteWampTopicSubscriber, EventDetails> Action { get; set; }

            public void Publish(IRemoteWampTopicSubscriber subscriber)
            {
                Action(subscriber, Details);
            }
        }
    }
}