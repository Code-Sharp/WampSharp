using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class PubSubSubjectTupleTests
    {
        [Test]
        public async Task PositionalTupleObservableOnNextPublishesEventWithPositionalArguments()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            var subject =
                publisher.RealmProxy.Services.GetSubject
                         ("com.myapp.mytopic2",
                          new MyPositionalTupleEventConverter());

            IWampTopicProxy topicProxy =
                subscriber.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.mytopic2");

            MyCustomSubscriber myCustomSubscriber = new MyCustomSubscriber();

            IAsyncDisposable subscribe =
                await topicProxy.Subscribe(myCustomSubscriber,
                                           new SubscribeOptions());

            // subject.OnNext(("Hello", 37, 23));
            subject.OnNext(ValueTuple.Create("Hello", 37, 23));

            Assert.That(myCustomSubscriber.ArgumentsKeywords, Is.Null.Or.Empty);

            ISerializedValue[] arguments = myCustomSubscriber.Arguments;

            Assert.That(arguments[0].Deserialize<string>(), Is.EqualTo("Hello"));
            Assert.That(arguments[1].Deserialize<int>(), Is.EqualTo(37));
            Assert.That(arguments[2].Deserialize<int>(), Is.EqualTo(23));
        }

        [Test]
        public async Task KeywordTupleObservableOnNextPublishesEventWithPositionalArguments()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            var subject =
                publisher.RealmProxy.Services.GetSubject
                         ("com.myapp.topic2",
                          new MyKeywordTupleEventConverter());

            IWampTopicProxy topicProxy =
                subscriber.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic2");

            MyCustomSubscriber myCustomSubscriber = new MyCustomSubscriber();

            IAsyncDisposable subscribe =
                await topicProxy.Subscribe(myCustomSubscriber,
                    new SubscribeOptions());

            MyClass instance = new MyClass()
            {
                Counter = 1,
                Foo = new[] { 1, 2, 3 }
            };

            // subject.OnNext((37, 23, "Hello", instance))
            subject.OnNext(ValueTuple.Create(37, 23, "Hello", instance));

            Assert.That(myCustomSubscriber.Arguments, Is.Empty);

            IDictionary<string, ISerializedValue> argumentsKeywords =
                myCustomSubscriber.ArgumentsKeywords;

            Assert.That(argumentsKeywords["number1"].Deserialize<int>(), Is.EqualTo(37));
            Assert.That(argumentsKeywords["number2"].Deserialize<int>(), Is.EqualTo(23));
            Assert.That(argumentsKeywords["c"].Deserialize<string>(), Is.EqualTo("Hello"));
            Assert.That(argumentsKeywords["d"].Deserialize<MyClass>(), Is.EqualTo(instance));
        }

        [Test]
        public async Task SubscriberGetsArgumentsArrayEventAsPositionalTuple()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            var subject =
                subscriber.RealmProxy.Services.GetSubject
                          ("com.myapp.topic2",
                           new MyPositionalTupleEventConverter());

            MyPositionalSubscriber mySubscriber = new MyPositionalSubscriber();

            subject.Subscribe(mySubscriber);

            IWampTopicProxy topicProxy =
                publisher.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic2");

            long? publish =
                await topicProxy.Publish(new PublishOptions(), new object[]
                                         {
                                             "Hello",
                                             47,
                                             23,
                                         });

            Assert.That(mySubscriber.Number1, Is.EqualTo(47));
            Assert.That(mySubscriber.Number2, Is.EqualTo(23));
            Assert.That(mySubscriber.C, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task SubscriberGetsArgumentsKeywordsEventAsKeywordTuple()
        {
            WampPlayground playground = new WampPlayground();

            PublisherSubscriber dualChannel = await playground.GetPublisherSubscriberDualChannel();
            IWampChannel publisher = dualChannel.Publisher;
            IWampChannel subscriber = dualChannel.Subscriber;

            var subject =
                subscriber.RealmProxy.Services.GetSubject
                          ("com.myapp.topic2",
                           new MyKeywordTupleEventConverter());

            MyKeywordSubscriber mySubscriber = new MyKeywordSubscriber();

            subject.Subscribe(mySubscriber);

            IWampTopicProxy topicProxy =
                publisher.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic2");

            MyClass instance = new MyClass()
            {
                Counter = 1,
                Foo = new[] { 1, 2, 3 }
            };

            long? publish =
                await topicProxy.Publish(new PublishOptions(),
                                         new object[0],
                                         new Dictionary<string, object>()
                                         {
                                             {"number1", 47},
                                             {"d", instance},
                                             {"c", "Hello"},
                                             {"number2", 23},
                                         });

            Assert.That(mySubscriber.Number1, Is.EqualTo(47));
            Assert.That(mySubscriber.Number2, Is.EqualTo(23));
            Assert.That(mySubscriber.C, Is.EqualTo("Hello"));
            Assert.That(mySubscriber.D, Is.EqualTo(instance));
        }

        public class MyCustomSubscriber : LocalSubscriber
        {
            private IDictionary<string, ISerializedValue> mArgumentsKeywords;

            public MyCustomSubscriber() : base()
            {
            }

            public override LocalParameter[] Parameters => new LocalParameter[0];

            public EventDetails Details { get; private set; }

            public ISerializedValue[] Arguments { get; private set; }

            public IDictionary<string, ISerializedValue> ArgumentsKeywords => mArgumentsKeywords;

            protected override void InnerEvent<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords)
            {
                if (arguments != null)
                {
                    Arguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x)).ToArray();
                }

                if (argumentsKeywords != null)
                {
                    mArgumentsKeywords =
                        argumentsKeywords.ToDictionary(x => x.Key,
                            x => (ISerializedValue)new SerializedValue<TMessage>(formatter, x.Value));
                }

                Details = details;
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
                return Equals((MyClass)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Counter * 397) ^ (Foo != null ? Foo.GetHashCode() : 0);
                }
            }
        }

        public class MyPositionalSubscriber : IObserver<(string c, int number1, int number2)>
        {
            public string C { get; set; }

            public int Number1 { get; set; }

            public int Number2 { get; set; }

            public void OnNext((string c, int number1, int number2) value)
            {
                (C, Number1, Number2) = value;
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }
        }

        public class MyKeywordSubscriber : IObserver<(int number1, int number2, string c, MyClass d)>
        {
            public int Number1 { get; set; }

            public int Number2 { get; set; }

            public string C { get; set; }

            public MyClass D { get; set; }

            public void OnNext((int number1, int number2, string c, MyClass d) value)
            {
                (Number1, Number2, C, D) = value;
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }
        }

        public class MyPositionalTupleEventConverter : WampEventValueTupleConverter<(string, int, int)>
        {
        }

        public class MyKeywordTupleEventConverter : WampEventValueTupleConverter<(int number1, int number2, string c, MyClass d)>
        {
        }
    }
}