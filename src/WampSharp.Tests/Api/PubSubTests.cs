using System;
using System.Reactive.Subjects;
using NUnit.Framework;
using WampSharp.PubSub.Server;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.TestHelpers.Integration;

namespace WampSharp.Tests.Api
{

    [TestFixture]
    public class PubSubTests
    {
        [Test]
        public void TopicOnNextCallsSubjectOnNext()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            IWampTopicContainer topicContainer = host.TopicContainer;

            host.Open();

            IWampChannel<MockRaw> channel =
                playground.CreateNewChannel();

            channel.Open();

            IWampTopic topic = null;

            topicContainer.TopicCreated +=
                (sender, args) => topic = args.Topic;

            object @event = null;
            string topicUri = "http://example.com/simple";
            ISubject<object> subject = channel.GetSubject<object>(topicUri);
            subject.Subscribe(x => @event = x);

            Assert.That(topic, Is.Not.Null);
            Assert.That(topic.TopicUri, Is.EqualTo(topicUri));

            string value = "Test :)";
            topic.OnNext(value);

            Assert.That(@event, Is.EqualTo(value));
        }

        [Test]
        public void OpenWillNotBlockOnConnectionLost()
        {
            var wampChannelFactory = new WampChannelFactory<MockRaw>(new MockRawFormatter());
            var mockControlledWampConnection = new MockControlledWampConnection<MockRaw>();
            
            var wampChannel = wampChannelFactory.CreateChannel(mockControlledWampConnection);
            var openAsync = wampChannel.OpenAsync();

            Assert.IsFalse(openAsync.IsCompleted);
            mockControlledWampConnection.OnCompleted();
            Assert.IsTrue(openAsync.IsCompleted);
        }
    }
}