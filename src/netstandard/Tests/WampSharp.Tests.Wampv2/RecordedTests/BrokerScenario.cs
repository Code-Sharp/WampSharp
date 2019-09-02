using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Serialization;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class BrokerScenario : BaseScenario
    {
        public IEnumerable<WampMessage<MockRaw>> Subscriptions { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Unsubscriptions { get; set; }

        public IEnumerable<WampMessage<MockRaw>> Publications { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Events { get; set; }

        public MockClient<IWampClientProxy<MockRaw>> Publisher { get; set; }
        public MockClient<IWampClientProxy<MockRaw>> Subscriber { get; set; }

        public IEnumerable<WampMessage<MockRaw>> PublicationAcks { get; set; }

        public IEnumerable<WampMessage<MockRaw>> PublicationErrors { get; set; }

        protected override object CreateServer()
        {
            WampMessageSerializerFactory serializerGenerator =
                new WampMessageSerializerFactory
                    (new WampOutgoingRequestSerializer<MockRaw>(Binding.Formatter));

            IWampEventSerializer eventSerializer =
                serializerGenerator.GetSerializer<IWampEventSerializer>();

            return new WampPubSubServer<MockRaw>(new WampTopicContainer(),
                                                 eventSerializer,
                                                 Binding,
                                                 new LooseUriValidator());
        }
    }
}