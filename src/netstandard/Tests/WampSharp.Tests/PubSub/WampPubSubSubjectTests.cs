using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;
using WampSharp.Tests.PubSub.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.PubSub.Client;

namespace WampSharp.Tests.PubSub
{
    [TestFixture]
    public class WampPubSubSubjectTests
    {
        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void BeforeSubscribeCallNoSubscriptionsAreMade(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            Assert.That(requestManager.Subscriptions, Is.Empty);
            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void SubscribeCallCallsServerProxySubscribe(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));

            WampSubscribeRequest<MockRaw> subscription =
                requestManager.Subscriptions.First();

            Assert.That(subscription.TopicUri, Is.EqualTo(topicUri));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void SubscribeCallsServerProxySubscribeOnceForAllSubscribers(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void DisposeDoesntCallServerProxyUnsubscribeIfObserversAreStillSubscribed(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            cancelation.Dispose();

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void DisposeCallsServerProxyUnsubscribeIfNoObserversAreSubscribed(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            cancelation.Dispose();
            cancelation2.Dispose();

            Assert.That(requestManager.SubscriptionRemovals.Count, Is.EqualTo(1));
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));

            WampSubscribeRequest<MockRaw> removal = 
                requestManager.SubscriptionRemovals.First();

            Assert.That(removal.TopicUri, Is.EqualTo(topicUri));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.EventMessages))]
        public void ClientEventCallsObserverOnNext(string topicUri, object @event)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            List<object> events = new List<object>();
            IDisposable cancelation = subject.Subscribe(x => events.Add(x));

            WampSubscribeRequest<MockRaw> subscription =
                requestManager.Subscriptions.First();

            subscription.Client.Event(topicUri, new MockRaw(@event));

            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0], Is.EqualTo(@event));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PublishMessagesSimple))]
        public void OnNextCallCallsServerProxyEvent(string topicUri, object @event)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });

            subject.OnNext(@event);

            Assert.That(requestManager.Publications.Count, Is.EqualTo(1));

            WampPublishRequest<MockRaw> publication =
                requestManager.Publications.FirstOrDefault();

            Assert.That(publication.Event, Is.EqualTo(@event));
            Assert.That(publication.ExcludeMe, Is.EqualTo(false)); // Our specification
            Assert.That(publication.TopicUri, Is.EqualTo(topicUri));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.UnsubscribeMessages))]
        public void OnCompletedCallCallsObserverOnCompleted(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            bool onCompleted = false;
            IDisposable cancelation = subject.Subscribe(x => { }, () => onCompleted = true);

            Assert.That(onCompleted, Is.EqualTo(false));
            subject.OnCompleted();
            Assert.That(onCompleted, Is.EqualTo(true));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.UnsubscribeMessages))]
        public void OnCompletedCallCallsServerProxyUnsubscribe(string topicUri)
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>(topicUri, DummyConnection<MockRaw>.Instance);

            IDisposable cancelation = subject.Subscribe(x => { });
            Assert.That(requestManager.SubscriptionRemovals.Count, Is.EqualTo(0));
            
            subject.OnCompleted();
            Assert.That(requestManager.SubscriptionRemovals.Count, Is.EqualTo(1));
            
            WampSubscribeRequest<MockRaw> removal = 
                requestManager.SubscriptionRemovals.First();

            Assert.That(removal.TopicUri, Is.EqualTo(topicUri));
        }

        private static WampPubSubClientFactory<MockRaw> GetClientFactory(MockWampPubSubRequestManager<MockRaw> requestManager)
        {
            WampPubSubClientFactory<MockRaw> clientFactory =
                new WampPubSubClientFactory<MockRaw>
                    (new MockWampPubSubServerProxyFactory
                         (client => requestManager.GetServer(client)),
                     new MockRawFormatter());

            return clientFactory;
        }
    }
}