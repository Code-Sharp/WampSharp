using System.Reflection;
using NUnit.Framework;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.Dispatch.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Dispatch
{
    [TestFixture]
    public class WampRequestMapperTests
    {
        private readonly WampRequestMapper<MockRaw> mServerMapper = 
            new WampRequestMapper<MockRaw>(typeof (MockWampServer),
            new MockRawFormatter());

        private readonly WampRequestMapper<MockRaw> mClientMapper = new WampRequestMapper<MockRaw>(typeof(MockWampClient),
            new MockRawFormatter());

        [TestCaseSource(typeof (Messages), nameof(Messages.PrefixMessages))]
        public void Prefix(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Prefix(default(IWampClient), default(string), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.CallMessages))]
        public void Call(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Call(default(IWampClient), default(string), default(string), default(MockRaw[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.SubscribeMessages))]
        public void Subscribe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Subscribe(default(IWampClient), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.UnsubscribeMessages))]
        public void Unsubscribe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Unsubscribe(default(IWampClient), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.PublishMessagesSimple))]
        public void Publish(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.PublishMessagesExcludeMe))]
        public void PublishExcludeMe(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(bool)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.PublishMessagesExclude))]
        public void PublishExcludeList(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(string[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.PublishMessagesEligible))]
        public void PublishEligibleList(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampServer<MockRaw> server) =>
                           server.Publish(default(IWampClient), default(string), default(MockRaw), default(string[]),
                                          default(string[])));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.WelcomeMessages))]
        public void Welcome(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mClientMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.Welcome(default(string), default(int), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.CallResultMessages))]
        public void CallResult(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mClientMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallResult(default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.CallErrorMessagesSimple))]
        public void CallErrorSimple(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mClientMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallError(default(string), default(string), default(string)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.CallErrorMessagesDetailed))]
        public void CallErrorDetailed(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mClientMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.CallError(default(string), default(string), default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof (Messages), nameof(Messages.EventMessages))]
        public void Event(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mClientMapper;

            WampMethodInfo mapped = mapper.Map(message);

            MethodInfo expected =
                Method.Get((IWampClient<MockRaw> client) =>
                           client.Event(default(string), default(MockRaw)));

            Assert.That(mapped.Method, Is.SameAs(expected));
        }

        [TestCaseSource(typeof(Messages), nameof(Messages.ClientMessages))]
        public void UnknownMessageReturnsNullIfMissingContractNotImplemented(WampMessage<MockRaw> message)
        {
            IWampRequestMapper<MockRaw> mapper =
                mServerMapper;

            WampMethodInfo mapped = mapper.Map(message);

            Assert.That(mapped, Is.Null);
        }

        [TestCaseSource(typeof(Messages), nameof(Messages.ClientMessages))]
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

        [TestCaseSource(typeof(Messages), nameof(Messages.ClientMessages))]
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