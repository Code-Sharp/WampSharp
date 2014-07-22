using System;
using System.Reactive.Subjects;
using NUnit.Framework;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V1;
using WampSharp.V1.PubSub.Server;

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
    }
}