using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemEx;
using Newtonsoft.Json;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class PubSubReflectionTests
    {
        [Test]
        public async void PublisherCustomDelegateEventRaisePublishesEventWithKeywordArguments()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            MyPublisher myPublisher = new MyPublisher();

            IDisposable disposable =
                publisher.RealmProxy.Services.RegisterPublisher
                    (myPublisher);

            IWampTopicProxy topicProxy =
                subscriber.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic2");

            MyCustomSubscriber myCustomSubscriber = new MyCustomSubscriber();

            IAsyncDisposable subscribe =
                await topicProxy.Subscribe(myCustomSubscriber,
                    new SubscribeOptions());

            MyClass instance = new MyClass()
            {
                Counter = 1,
                Foo = new[] {1, 2, 3}
            };

            myPublisher.RaiseMyEvent(37, 23, "Hello", instance);

            Assert.That(myCustomSubscriber.Arguments, Is.Empty);
            
            IDictionary<string, ISerializedValue> argumentsKeywords = 
                myCustomSubscriber.ArgumentsKeywords;

            Assert.That(argumentsKeywords["number1"].Deserialize<int>(), Is.EqualTo(37));
            Assert.That(argumentsKeywords["number2"].Deserialize<int>(), Is.EqualTo(23));
            Assert.That(argumentsKeywords["c"].Deserialize<string>(), Is.EqualTo("Hello"));
            Assert.That(argumentsKeywords["d"].Deserialize<MyClass>(), Is.EqualTo(instance));
        }

        [Test]
        public async void PublisherActionDelegateEventRaisePublishesEventWithPositionalArguments()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            MyOtherPublisher myPublisher = new MyOtherPublisher();

            IDisposable disposable =
                publisher.RealmProxy.Services.RegisterPublisher
                    (myPublisher);

            IWampTopicProxy topicProxy =
                subscriber.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.mytopic2");

            MyCustomSubscriber myCustomSubscriber = new MyCustomSubscriber();

            IAsyncDisposable subscribe =
                await topicProxy.Subscribe(myCustomSubscriber,
                    new SubscribeOptions());

            myPublisher.RaiseMyEvent("Hello", 37, 23);

            Assert.That(myCustomSubscriber.ArgumentsKeywords, Is.Null.Or.Empty);

            ISerializedValue[] arguments = myCustomSubscriber.Arguments;

            Assert.That(arguments[0].Deserialize<string>(), Is.EqualTo("Hello"));
            Assert.That(arguments[1].Deserialize<int>(), Is.EqualTo(37));
            Assert.That(arguments[2].Deserialize<int>(), Is.EqualTo(23));
        }

        [Test]
        public async void SubscriberGetsEventAsParameters()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            MySubscriber mySubscriber = new MySubscriber();

            IAsyncDisposable disposable =
                await subscriber.RealmProxy.Services.RegisterSubscriber
                    (mySubscriber);

            IWampTopicProxy topicProxy =
                publisher.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic2");

            MyClass instance = new MyClass()
            {
                Counter = 1,
                Foo = new[] {1, 2, 3}
            };

            long? publish =
                await topicProxy.Publish(new PublishOptions(), new object[]
                {
                    47,
                    23,
                },
                    new Dictionary<string, object>()
                    {
                        {"d", instance},
                        {"c", "Hello"}
                    });


            Assert.That(mySubscriber.Number1, Is.EqualTo(47));
            Assert.That(mySubscriber.Number2, Is.EqualTo(23));
            Assert.That(mySubscriber.C, Is.EqualTo("Hello"));
            Assert.That(mySubscriber.D, Is.EqualTo(instance));
        }

        [Test]
        [TestCase(null)]
        [TestCase(false)]
        [TestCase(true)]
        public async void SubscriberGetsEventContextWithPublicationId(bool? acknowledge)
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            MyOtherSubscriber mySubscriber = new MyOtherSubscriber();

            IAsyncDisposable disposable =
                await subscriber.RealmProxy.Services.RegisterSubscriber
                    (mySubscriber);

            IWampTopicProxy topicProxy =
                publisher.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.mytopic2");

            long? publish =
                await topicProxy.Publish
                    (new PublishOptions {Acknowledge = acknowledge},
                        new object[] {47, 23, "Hello"});

            if (acknowledge == true)
            {
                Assert.That(publish, Is.EqualTo(mySubscriber.EventContext.PublicationId));                
            }
            else
            {
                Assert.That(mySubscriber.EventContext.PublicationId, Is.Not.Null);                                
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        public async void SubscriberGetsEventContextWithPublisherId(bool? discloseMe)
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            MyOtherSubscriber mySubscriber = new MyOtherSubscriber();

            IAsyncDisposable disposable =
                await subscriber.RealmProxy.Services.RegisterSubscriber
                    (mySubscriber);

            IWampTopicProxy topicProxy =
                publisher.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.mytopic2");

            long? publish =
                await topicProxy.Publish
                    (new PublishOptions { DiscloseMe = discloseMe },
                        new object[] { 47, 23, "Hello" });

            long? publisherId = mySubscriber.EventContext.EventDetails.Publisher;

            if (discloseMe == true)
            {
                Assert.That(publisherId, Is.EqualTo(dualChannel.PublisherSessionId));
            }
            else
            {
                Assert.That(publisherId, Is.EqualTo(null));                
            }
        }

        public class MyCustomSubscriber : LocalSubscriber
        {
            private EventDetails mDetails;
            private ISerializedValue[] mArguments;
            private IDictionary<string, ISerializedValue> mArgumentsKeywords;

            public MyCustomSubscriber() : base()
            {
            }

            public override LocalParameter[] Parameters
            {
                get { return new LocalParameter[0];}
            }

            public EventDetails Details
            {
                get { return mDetails; }
            }

            public ISerializedValue[] Arguments
            {
                get { return mArguments; }
            }

            public IDictionary<string, ISerializedValue> ArgumentsKeywords
            {
                get { return mArgumentsKeywords; }
            }

            protected override void InnerEvent<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords)
            {
                if (arguments != null)
                {
                    mArguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x)).ToArray();                    
                }

                if (argumentsKeywords != null)
                {
                    mArgumentsKeywords =
                        argumentsKeywords.ToDictionary(x => x.Key,
                            x => (ISerializedValue)new SerializedValue<TMessage>(formatter, x.Value));
                }

                mDetails = details;
            }
        }

        public class MyClass
        {
            [JsonProperty("counter")]
            public int Counter { get; set; }

            [JsonProperty("foo")]
            public int[] Foo { get; set; }

            protected bool Equals(MyClass other)
            {
                return Counter == other.Counter && Foo.SequenceEqual(other.Foo);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MyClass) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Counter*397) ^ (Foo != null ? Foo.GetHashCode() : 0);
                }
            }
        }

        public delegate void MyPublicationDelegate(int number1, int number2, string c, MyClass d);

        public interface IMyPublisher
        {
            [WampTopic("com.myapp.topic2")]
            event MyPublicationDelegate MyEvent;
        }

        public class MyPublisher : IMyPublisher
        {
            public void RaiseMyEvent(int number1, int number2, string c, MyClass d)
            {
                MyPublicationDelegate handler = MyEvent;

                if (handler != null)
                {
                    handler(number1, number2, c, d);
                }
            }

            public event MyPublicationDelegate MyEvent;
        }

        public class MyOtherPublisher
        {
            [WampTopic("com.myapp.mytopic2")]
            public event Action<string, int, int> MyEvent;

            public void RaiseMyEvent(string arg1, int arg2, int arg3)
            {
                var handler = MyEvent;
                if (handler != null) handler(arg1, arg2, arg3);
            }
        }

        public class MyOtherSubscriber
        {
            public WampEventContext EventContext { get; set; }

            [WampTopic("com.myapp.mytopic2")]
            public void OnMyTopic(int number1, int number2, string c)
            {
                EventContext = WampEventContext.Current;
            }
        }

        public class MySubscriber
        {
            public int Number1 { get; set; }

            public int Number2 { get; set; }

            public string C { get; set; }

            public MyClass D { get; set; }

            [WampTopic("com.myapp.topic2")]
            public void OnTopic2(int number1, int number2, string c, MyClass d)
            {
                Number1 = number1;
                Number2 = number2;
                C = c;
                D = d;
            }
        }
    }
}