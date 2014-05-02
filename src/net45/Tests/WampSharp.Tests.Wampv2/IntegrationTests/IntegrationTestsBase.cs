using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class IntegrationTestsBase
    {
        protected MessageMapper mMapper;

        [SetUp]
        public void Setup()
        {
            mMapper = new MessageMapper();
        }

        protected static IEnumerable<WampMessage<MockRaw>> GetCalls(Type scenario, Channel channel, WampMessageType[] category)
        {
            Type calleeToDealer = scenario.GetNestedType(channel.ToString());

            PropertyInfo callsProperty =
                calleeToDealer.GetProperty("Calls", BindingFlags.Static |
                                                    BindingFlags.Public);

            IEnumerable<WampMessage<MockRaw>> calls =
                callsProperty.GetValue(null, null) as IEnumerable<WampMessage<MockRaw>>;

            var filtered =
                calls.Where(x => category.Contains(x.MessageType))
                     .Select(x => new MockWampMessage(x));

            return filtered;
        }
    }
}