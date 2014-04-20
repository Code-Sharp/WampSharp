using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.Tests.Wampv2.MockBuilder;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    [TestFixture]
    internal class BrokerIntegrationTests : IntegrationTestsBase
    {
        [TestCaseSource("TestCases")]
        [Test]
        public void BrokerTest(BrokerScenario scenario)
        {
            foreach (var subscription in scenario.Subscriptions)
            {
                scenario.Handler.HandleMessage(scenario.Subscriber.Client, subscription);
            }

            foreach (var publication in scenario.Publications)
            {
                scenario.Handler.HandleMessage(scenario.Publisher.Client, publication);                
            }

            foreach (WampMessage<MockRaw> response in scenario.Events)
            {
                IEnumerable<WampMessage<MockRaw>> recordedMessages =
                    scenario.Subscriber.Recorder.RecordedMessages;

                WampMessage<MockRaw> mapped =
                    mMapper.MapRequest(response, recordedMessages, true);

                if (mapped == null)
                {
                    Assert.Fail("An expected response of the form {0} was not found", response);
                }
            }

            foreach (WampMessage<MockRaw> response in scenario.PublicationAcks
                                                              .Concat(scenario.PublicationErrors)) 
            {
                IEnumerable<WampMessage<MockRaw>> recordedMessages =
                    scenario.Publisher.Recorder.RecordedMessages;

                WampMessage<MockRaw> mapped =
                    mMapper.MapRequest(response, recordedMessages, false);

                if (mapped == null)
                {
                    Assert.Fail("An expected response of the form {0} was not found", response);
                }
            }
        }


        public IEnumerable<TestCaseData> TestCases()
        {
            foreach (Type nestedType in typeof (TestHelpers.PubSub).GetNestedTypes())
            {
                var publications =
                    GetCalls(nestedType, Channel.PublisherToBroker, new[] {WampMessageType.v2Publish});

                var publicationAcks =
                    GetCalls(nestedType, Channel.BrokerToPublisher, new[] {WampMessageType.v2Published})
                        .ToArray();

                var publicationErrors =
                    GetCalls(nestedType, Channel.BrokerToPublisher, new[] {WampMessageType.v2Error});

                var subscriptions =
                    GetCalls(nestedType, Channel.SubscriberToBroker, new[] {WampMessageType.v2Subscribe});

                var subscriptionAcks =
                    GetCalls(nestedType, Channel.BrokerToSubscriber, new[] { WampMessageType.v2Subscribed })
                    .ToArray();

                var events =
                    GetCalls(nestedType, Channel.BrokerToSubscriber, new[] {WampMessageType.v2Event})
                        .ToArray();

                BrokerScenario scenario = new BrokerScenario();

                scenario.Publications = publications;
                scenario.PublicationAcks = publicationAcks;
                scenario.PublicationErrors = publicationErrors;
                scenario.Subscriptions = subscriptions;
                scenario.Events = events;

                MockClient<IWampClient<MockRaw>> subscriber = GetSubscriber(nestedType, scenario.ClientBuilder, scenario.Handler, events.Concat(subscriptionAcks));
                MockClient<IWampClient<MockRaw>> publisher = GetPublisher(nestedType, scenario.ClientBuilder, publicationAcks);

                scenario.Subscriber = subscriber;
                scenario.Publisher = publisher;

                TestCaseData testCase = new TestCaseData(scenario);
                testCase.SetName(string.Format("PubSubIntegrationTest.{0}",
                                               nestedType.Name));

                yield return testCase;
            }
        }

        private static MockClient<IWampClient<MockRaw>> GetPublisher(Type scenario, WampMockClientBuilder<MockRaw> builder, IEnumerable<WampMessage<MockRaw>> calls)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.BrokerToPublisher,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            long sessionId = (long)welcome.Arguments[0].Value;

            NullPlayer<MockRaw> nullPlayer =
                new NullPlayer<MockRaw>();

            IMessageRecorder<MockRaw> recorder =
                new ResponsiveMessageRecorder(calls,
                                        new Dictionary<WampMessageType, string>()
                                            {
                                                {WampMessageType.v2Published, "publicationId"}
                                            });

            IWampClient<MockRaw> built =
                builder.Create(sessionId, nullPlayer,
                                      recorder);

            MockClient<IWampClient<MockRaw>> result =
                new MockClient<IWampClient<MockRaw>>(built, recorder);

            return result;
        }

        private static MockClient<IWampClient<MockRaw>> GetSubscriber(Type scenario, WampMockClientBuilder<MockRaw> clientBuilder, IWampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> handler, IEnumerable<WampMessage<MockRaw>> calls)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.BrokerToSubscriber,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            long sessionId = (long)welcome.Arguments[0].Value;

            // TODO: After enough events unsubscribe.
            NullPlayer<MockRaw> nullPlayer =
                new NullPlayer<MockRaw>();

            IMessageRecorder<MockRaw> recorder =
                new ResponsiveMessageRecorder(calls,
                                        new Dictionary<WampMessageType, string>()
                                            {
                                                {WampMessageType.v2Subscribed, "subscriptionId"}
                                            });

            IWampClient<MockRaw> built =
                clientBuilder.Create(sessionId, nullPlayer,
                                      recorder);

            MockClient<IWampClient<MockRaw>> result =
                new MockClient<IWampClient<MockRaw>>(built, recorder);

            return result;
        }
    }
}