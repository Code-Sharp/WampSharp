using System.Reflection;
using NUnit.Framework;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.Dispatch.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts.V1;

namespace WampSharp.Tests.Dispatch
{
    [TestFixture]
    public class WampRequestMapperTests
    {
        [TestCaseSource(typeof (Messages), "PrefixMessages")]
        public void Prefix(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Prefix(default(IWampClient), default(string), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "CallMessages")]
        public void Call(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Call(default(IWampClient), default(string), default(string), default(MockRaw[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "SubscribeMessages")]
        public void Subscribe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Subscribe(default(IWampClient), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "UnsubscribeMessages")]
        public void Unsubscribe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Unsubscribe(default(IWampClient), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "PublishMessagesSimple")]
        public void Publish(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "PublishMessagesExcludeMe")]
        public void PublishExcludeMe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(bool)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "PublishMessagesExclude")]
        public void PublishExcludeList(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(string[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "PublishMessagesEligible")]
        public void PublishEligibleList(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(string[]),
                                          default(string[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "WelcomeMessages")]
        public void Welcome(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.Welcome(default(string), default(int), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "CallResultMessages")]
        public void CallResult(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallResult(default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "CallErrorMessagesSimple")]
        public void CallErrorSimple(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallError(default(string), default(string), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "CallErrorMessagesDetailed")]
        public void CallErrorDetailed(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallError(default(string), default(string), default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), "EventMessages")]
        public void Event(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof (MockWampClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.Event(default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof(Messages), "ClientMessages")]
        public void UnknownMessageReturnsNullIfMissingContractNotImplemented(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampServer),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            Assert.That(mapped, Is.Null);
        }

        [TestCaseSource(typeof(Messages), "ClientMessages")]
        public void UnknownMessageReturnsMissingIfMissingContractImplemented(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampServerWithMissing),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampMissingMethodContract<MockRaw> server) =>
                           server.Missing(default(WampMessage<MockRaw>)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof(Messages), "ClientMessages")]
        public void UnknownMessageReturnsMissingIfMissingContractClientVersionImplemented(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                new WampRequestMapper<MockRaw>(typeof(MockWampServerWithMissingClient),
                                               new MockRawFormatter());

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampMissingMethodContract<MockRaw, IWampClient> server) =>
                           server.Missing(default(IWampClient), default(WampMessage<MockRaw>)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }
    }
}