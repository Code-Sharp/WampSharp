using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class BrokerScenario : BaseScenario
    {
        public IEnumerable<WampMessage<MockRaw>> Subscriptions { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Unsubscriptions { get; set; }

        public IEnumerable<WampMessage<MockRaw>> Publications { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Events { get; set; }

        public MockClient<IWampClient<MockRaw>> Publisher { get; set; }
        public MockClient<IWampClient<MockRaw>> Subscriber { get; set; }

        public IEnumerable<WampMessage<MockRaw>> PublicationAcks { get; set; }

        public IEnumerable<WampMessage<MockRaw>> PublicationErrors { get; set; }

        protected override object CreateServer()
        {
            WampMessageSerializerBuilder<MockRaw> serializerGenerator =
                new WampMessageSerializerBuilder<MockRaw>
                    (new WampOutgoingRequestSerializer<MockRaw>(Binding.Formatter));

            IWampEventSerializer<MockRaw> eventSerializer =
                serializerGenerator.GetSerializer<IWampEventSerializer<MockRaw>>();

            return new WampPubSubServer<MockRaw>(new WampTopicContainer(),
                                                 eventSerializer,
                                                 Binding);
        }
    }
}