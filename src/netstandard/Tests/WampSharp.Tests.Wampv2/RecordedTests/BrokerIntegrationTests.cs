using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    [TestFixture]
    internal class BrokerIntegrationTests : IntegrationTestsBase
    {
        [TestCaseSource(nameof(TestCases))]
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


        public static IEnumerable<TestCaseData> TestCases()
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

                MockClient<IWampClientProxy<MockRaw>> subscriber = GetSubscriber(nestedType, scenario.ClientBuilder, scenario.Handler, events.Concat(subscriptionAcks));
                MockClient<IWampClientProxy<MockRaw>> publisher = GetPublisher(nestedType, scenario.ClientBuilder, publicationAcks);

                scenario.Subscriber = subscriber;
                scenario.Publisher = publisher;

                TestCaseData testCase = new TestCaseData(scenario);
                testCase.SetName($"PubSubIntegrationTest.{nestedType.Name}");

                yield return testCase;
            }
        }

        private static MockClient<IWampClientProxy<MockRaw>> GetPublisher(Type scenario, WampMockClientBuilder<MockRaw> builder, IEnumerable<WampMessage<MockRaw>> calls)
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

            IWampClientProxy<MockRaw> built =
                builder.Create(nullPlayer,
                               recorder,
                               welcome);

            MockClient<IWampClientProxy<MockRaw>> result =
                new MockClient<IWampClientProxy<MockRaw>>(built, recorder);

            return result;
        }

        private static MockClient<IWampClientProxy<MockRaw>> GetSubscriber(Type scenario, WampMockClientBuilder<MockRaw> clientBuilder, IWampIncomingMessageHandler<MockRaw, IWampClientProxy<MockRaw>> handler, IEnumerable<WampMessage<MockRaw>> calls)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.BrokerToSubscriber,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            // TODO: After enough events unsubscribe.
            NullPlayer<MockRaw> nullPlayer =
                new NullPlayer<MockRaw>();

            IMessageRecorder<MockRaw> recorder =
                new ResponsiveMessageRecorder(calls,
                                        new Dictionary<WampMessageType, string>()
                                            {
                                                {WampMessageType.v2Subscribed, "subscriptionId"}
                                            });

            IWampClientProxy<MockRaw> built =
                clientBuilder.Create(nullPlayer,
                                      recorder,
                                      welcome);

            MockClient<IWampClientProxy<MockRaw>> result =
                new MockClient<IWampClientProxy<MockRaw>>(built, recorder);

            return result;
        }
    }
}