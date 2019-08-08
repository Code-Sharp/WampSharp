using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class PubSubRetainTests
    {
        [Test]
        [TestCase(true, true, 1, null)]
        [TestCase(true, true, 1, null)]
        [TestCase(false, true, 0, null)]
        [TestCase(true, false, 0, null)]
        [TestCase(false, false, 0, null)]
        [TestCase(null, true, 0, null)]
        [TestCase(true, null, 0, null)]
        [TestCase(null, null, 0, null)]
        public async Task SubscriberGetsRetainedEvent(bool? publishRetained, bool? getRetained, int expectedCount, bool? acknowledge = null)
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            ChannelWithExtraData publisher = await playground.GetChannel().ConfigureAwait(false);
            ChannelWithExtraData subscriber = await playground.GetChannel().ConfigureAwait(false);

            IWampTopicProxy topicProxy =
                publisher.Channel.RealmProxy.TopicContainer.GetTopicByUri
                ("com.myapp.mytopic2");

            long? lastPublicationId = null;

            for (int i = 0; i <= 42; i++)
            {
                lastPublicationId =
                    await topicProxy.Publish
                    (new PublishOptions
                     {
                         Retain = publishRetained,
                         Acknowledge = acknowledge
                     },
                     new object[] {i, 23, "Hello"});
            }

            publisher.Channel.Close();

            MyOtherSubscriber mySubscriber = new MyOtherSubscriber();

            IAsyncDisposable disposable =
                await subscriber.Channel.RealmProxy.Services.RegisterSubscriber(mySubscriber,
                new SubscriberRegistrationInterceptor(new SubscribeOptions(){GetRetained = getRetained}))
                                .ConfigureAwait(false);

            Assert.That(mySubscriber.Parameters.Count, Is.EqualTo(expectedCount));

            if (expectedCount == 1)
            {
                MyTopic2Parameters actual = mySubscriber.Parameters[0];
                Assert.That(actual.Number1, Is.EqualTo(42));
                Assert.That(actual.Number2, Is.EqualTo(23));
                Assert.That(actual.C, Is.EqualTo("Hello"));
                Assert.That(actual.EventContext.EventDetails.Retained, Is.EqualTo(true));

                if (acknowledge == true)
                {
                    Assert.That(actual.EventContext.PublicationId, Is.EqualTo(lastPublicationId));
                }
            }
        }

        private class MyOtherSubscriber
        {
            public List<MyTopic2Parameters> Parameters { get; } = new List<MyTopic2Parameters>();

            [WampTopic("com.myapp.mytopic2")]
            public void OnMyTopic(int number1, int number2, string c)
            {
                Parameters.Add(new MyTopic2Parameters(number1, number2, c, WampEventContext.Current));
            }
        }


        internal class MyTopic2Parameters
        {
            public MyTopic2Parameters(int number1, int number2, string c, WampEventContext eventContext)
            {
                Number1 = number1;
                Number2 = number2;
                C = c;
                EventContext = eventContext;
            }

            public int Number1 { get; }
            public int Number2 { get; }
            public string C { get; }
            public WampEventContext EventContext { get; }
        }
    }
}