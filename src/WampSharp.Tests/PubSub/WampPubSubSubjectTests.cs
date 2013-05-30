using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;
using WampSharp.PubSub.Client;
using WampSharp.Tests.PubSub.Helpers;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.PubSub
{
    [TestFixture]
    public class WampPubSubSubjectTests
    {
        [Test]
        public void BeforeSubscribeCallNoSubscriptionsAreMade()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            Assert.That(requestManager.Subscriptions, Is.Empty);
            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
        }

        [Test]
        public void SubscribeCallCallsServerProxySubscribe()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            IDisposable cancelation = subject.Subscribe(x => { });

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));

            WampSubscribeRequest<MockRaw> subscription =
                requestManager.Subscriptions.First();

            Assert.That(subscription.TopicUri, Is.EqualTo("myTopic"));
        }

        [Test]
        public void SubscribeCallsServerProxySubscribeOnceForAllSubscribers()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));
        }

        [Test]
        public void DisposeDoesntCallServerProxyUnsubscribeIfObserversAreStillSubscribed()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            cancelation.Dispose();

            Assert.That(requestManager.SubscriptionRemovals, Is.Empty);
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));
        }

        [Test]
        public void DisposeCallsServerProxyUnsubscribeIfNoObserversAreSubscribed()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            IDisposable cancelation = subject.Subscribe(x => { });
            IDisposable cancelation2 = subject.Subscribe(x => { });

            cancelation.Dispose();
            cancelation2.Dispose();

            Assert.That(requestManager.SubscriptionRemovals.Count, Is.EqualTo(1));
            Assert.That(requestManager.Publications, Is.Empty);
            Assert.That(requestManager.Subscriptions.Count, Is.EqualTo(1));

            WampSubscribeRequest<MockRaw> removal = 
                requestManager.SubscriptionRemovals.First();

            Assert.That(removal.TopicUri, Is.EqualTo("myTopic"));
        }

        [Test]
        public void ClientEventCallsObserverOnNext()
        {
            MockWampPubSubRequestManager<MockRaw> requestManager =
                new MockWampPubSubRequestManager<MockRaw>();

            WampPubSubClientFactory<MockRaw> clientFactory = GetClientFactory(requestManager);

            ISubject<object> subject = clientFactory.GetSubject<object>("myTopic");

            List<object> events = new List<object>();
            IDisposable cancelation = subject.Subscribe(x => events.Add(x));

            WampSubscribeRequest<MockRaw> subscription =
                requestManager.Subscriptions.First();

            subscription.Client.Event("myTopic", new MockRaw(13));

            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0], Is.EqualTo(13));
        }

        private static WampPubSubClientFactory<MockRaw> GetClientFactory(MockWampPubSubRequestManager<MockRaw> requestManager)
        {
            WampPubSubClientFactory<MockRaw> clientFactory =
                new WampPubSubClientFactory<MockRaw>
                    (new MockWampPubSubServerProxyFactory<MockRaw>
                         (client => requestManager.GetServer(client)),
                     new MockRawFormatter());
            return clientFactory;
        }
    }
}