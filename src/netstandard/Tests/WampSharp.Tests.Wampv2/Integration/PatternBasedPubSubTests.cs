using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class PatternBasedPubSubTests
    {
        [Test]
        public async Task PrefixPubSubTest()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = 
                await playground.GetPublisherSubscriberDualChannel();

            IWampChannel publisherChannel = dualChannel.Publisher;
            IWampChannel subscriberChannel = dualChannel.Subscriber;

            var mySubscriber = new MyPrefixSubscriber();

            Task<IAsyncDisposable> subscribeTask =
                subscriberChannel
                    .RealmProxy.Services.RegisterSubscriber
                    (mySubscriber,
                     new SubscriberRegistrationInterceptor(
                         new SubscribeOptions() {Match = "prefix"}));

            await subscribeTask;

            string msg = "Hello prefix pattern subscriber!";
            int counter = 0;

            // these are all received
            Publish(publisherChannel, "com.myapp.topic1.foobar", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp.topic1", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp.topi", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp2.foobar", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp2", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp.2", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myapp", new object[] {msg, counter++});

            // these are not received
            Publish(publisherChannel, "com.app.topic1", new object[] {msg, counter++});
            Publish(publisherChannel, "com.myap", new object[] {msg, counter++});
            Publish(publisherChannel, "com", new object[] {msg, counter++});

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 7), mySubscriber.Results);
        }

        [Test]
        public async Task WildCardPubSubTest()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel =
                await playground.GetPublisherSubscriberDualChannel();

            IWampChannel publisherChannel = dualChannel.Publisher;
            IWampChannel subscriberChannel = dualChannel.Subscriber;

            var mySubscriber = new MyWildcardSubscriber();

            Task<IAsyncDisposable> subscribeTask =
                subscriberChannel
                    .RealmProxy.Services.RegisterSubscriber
                    (mySubscriber,
                     new SubscriberRegistrationInterceptor(
                         new SubscribeOptions() { Match = "wildcard" }));

            await subscribeTask;

            string msg = "Hello wildcard pattern subscriber!";
            int counter = 0;

            // these are all received
            Publish(publisherChannel, "com.example.foobar.create", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example.1.create", new object[] { msg, counter++ });

            // these are not received
            Publish(publisherChannel, "com.example.foobar.delete", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example.foobar.create2", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example.foobar.create.barbaz", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example.foobar", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example.create", new object[] { msg, counter++ });
            Publish(publisherChannel, "com.example", new object[] { msg, counter++ });

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 2), mySubscriber.Results);
        }

        private static void Publish(IWampChannel publisherChannel, string topicUri, object[] arguments)
        {
            IWampTopicProxy topicProxy = 
                publisherChannel.RealmProxy.TopicContainer.GetTopicByUri(topicUri);
            
            Task<long?> task = topicProxy.Publish(new PublishOptions(), arguments);
        }

        private class MyPrefixSubscriber
        {
            private readonly List<int> mResults = new List<int>();

            public IEnumerable<int> Results => mResults;

            [WampTopic("com.myapp")]
            public void OnEvent(string msg, int counter)
            {
                Assert.That(msg, Is.EqualTo("Hello prefix pattern subscriber!"));

                mResults.Add(counter);
            }
        }

        private class MyWildcardSubscriber
        {
            private readonly List<int> mResults = new List<int>();

            public IEnumerable<int> Results => mResults;

            [WampTopic("com.example..create")]
            public void OnEvent(string msg, int counter)
            {
                Assert.That(msg, Is.EqualTo("Hello wildcard pattern subscriber!"));

                mResults.Add(counter);
            }
        }
    }
}