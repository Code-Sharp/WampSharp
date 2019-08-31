using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2.MetaApi;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class TestamentServiceTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BasicTestamentServiceTest(bool flushTestaments)
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.RealmContainer.GetRealmByName("realm1")
                      .HostTestamentService();

            PublisherSubscriber publisherSubscriber =
                await playground.GetPublisherSubscriberDualChannel();

            MySubscriber mySubscriber = new MySubscriber();

            await publisherSubscriber.Subscriber.RealmProxy.Services
                .RegisterSubscriber(mySubscriber);

            MyClass instance = new MyClass() {Counter = 1, Foo = new int[] {1, 2, 3}};
            await publisherSubscriber
                .Publisher.RealmProxy.GetTestamentServiceProxy()
                .AddTestamentAsync("com.myapp.topic2", new object[]
                {
                    47,
                    23
                }, new Dictionary<string, object>()
                {
                    {
                        "c", "Hello"
                    },
                    {"d", instance}
                });

            Assert.That(mySubscriber.Counter, Is.EqualTo(0));

            if (flushTestaments)
            {
                await publisherSubscriber.Publisher.RealmProxy.GetTestamentServiceProxy().FlushTestamentsAsync();
            }

            publisherSubscriber.Publisher.Close();

            if (flushTestaments)
            {
                Assert.That(mySubscriber.Counter, Is.EqualTo(0));
            }
            else
            {
                Assert.That(mySubscriber.Counter, Is.EqualTo(1));

                Assert.That(mySubscriber.Number1, Is.EqualTo(47));
                Assert.That(mySubscriber.Number2, Is.EqualTo(23));
                Assert.That(mySubscriber.C, Is.EqualTo("Hello"));
                Assert.That(mySubscriber.D, Is.EqualTo(instance));
            }
        }

        private class MySubscriber
        {
            public int Number1 { get; private set; }
            public int Number2 { get; private set; }
            public string C { get; private set; }
            public MyClass D { get; private set; }
            public int Counter { get; private set; }

            [WampTopic("com.myapp.topic2")]
            public void OnPublish(int number1, int number2, string c, MyClass d)
            {
                Counter++;
                D = d;
                C = c;
                Number2 = number2;
                Number1 = number1;
            }
        }

        private class MyClass
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
    }
}