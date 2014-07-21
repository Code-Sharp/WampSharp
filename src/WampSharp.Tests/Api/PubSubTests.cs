using System;
using System.Reactive.Subjects;
using NUnit.Framework;
using WampSharp.PubSub.Server;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Tests.Api
{
    using System.Threading.Tasks;

    [TestFixture]
    public class PubSubTests
    {
        private class MockControlledWampConnection<TMessage> : IControlledWampConnection<TMessage>
        {
            private readonly Subject<WampMessage<TMessage>> mWampConnection = new Subject<WampMessage<TMessage>>();

            public void OnCompleted()
            {
                mWampConnection.OnCompleted();
            }

            public void OnError(Exception error)
            {
                mWampConnection.OnError(error);
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                mWampConnection.OnNext(value);
            }

            public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
            {
                return mWampConnection.Subscribe(observer);
            }

            public void Connect()
            {
                mWampConnection.OnCompleted();
            }
        }

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
            
            var wampChannel = wampChannelFactory.CreateChannel(new MockControlledWampConnection<MockRaw>());
            var startNew = Task.Factory.StartNew(wampChannel.Open);
            startNew.Wait(100);

            Assert.IsTrue(startNew.IsCompleted);
        }
    }
}