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
    internal class DealerIntegrationTests : IntegrationTestsBase
    {
        [TestCaseSource(nameof(TestCases))]
        [Test]
        public void DealerTest(DealerScenario scenario)
        {
            foreach (var registration in scenario.Registrations)
            {
                scenario.Handler.HandleMessage(scenario.Callee.Client, registration);
            }

            scenario.Handler.HandleMessage(scenario.Caller.Client, scenario.Call.Request);

            foreach (WampMessage<MockRaw> response in scenario.Call.Responses)
            {
                IEnumerable<WampMessage<MockRaw>> recordedMessages = 
                    scenario.Caller.Recorder.RecordedMessages;

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
            foreach (Type nestedType in typeof (TestHelpers.Rpc).GetNestedTypes())
            {
                var scenarioCalls =
                    GetCalls(nestedType, Channel.CallerToDealer, MessageTypes.Rpc)
                        .Concat(GetCalls(nestedType, Channel.DealerToCaller, MessageTypes.Rpc))
                        .GroupBy(x => x.GetRequestId())
                        .Where(x => x.Key != null);

                var registrations =
                    GetCalls(nestedType, Channel.CalleeToDealer,
                             new WampMessageType[] {WampMessageType.v2Register});

                foreach (var currentCase in scenarioCalls)
                {
                    DealerScenario scenario = new DealerScenario();

                    MockClient<IWampClientProxy<MockRaw>> callee = GetCallee(nestedType, scenario.ClientBuilder, scenario.Handler);
                    MockClient<IWampClientProxy<MockRaw>> caller = GetCaller(nestedType, scenario.ClientBuilder);

                    WampMessage<MockRaw> request =
                        currentCase.FirstOrDefault(x => x.MessageType == WampMessageType.v2Call);

                    WampMessage<MockRaw>[] responses =
                        currentCase.Where(x => x.MessageType != WampMessageType.v2Call)
                                   .ToArray();

                    scenario.Call = new DealerCall()
                                        {
                                            Request = request,
                                            Responses = responses
                                        };
                    scenario.Registrations = registrations;
                    scenario.Callee = callee;
                    scenario.Caller = caller;

                    TestCaseData testCase = new TestCaseData(scenario);
                    testCase.SetName($"DealerIntegrationTest.{nestedType.Name}.{request.Arguments[2].Value}");

                    yield return testCase;
                }
            }
        }

        private static MockClient<IWampClientProxy<MockRaw>> GetCaller(Type scenario, WampMockClientBuilder<MockRaw> builder)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.DealerToCaller,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            NullPlayer<MockRaw> nullPlayer =
                new NullPlayer<MockRaw>();

            IMessageRecorder<MockRaw> messageRecorder =
                new MessageRecorder<MockRaw>();

            IWampClientProxy<MockRaw> built =
                builder.Create(nullPlayer,
                               messageRecorder,
                               welcome);

            MockClient<IWampClientProxy<MockRaw>> result =
                new MockClient<IWampClientProxy<MockRaw>>(built, messageRecorder);

            return result;
        }

        private static MockClient<IWampClientProxy<MockRaw>> GetCallee(Type scenario, WampMockClientBuilder<MockRaw> clientBuilder, IWampIncomingMessageHandler<MockRaw, IWampClientProxy<MockRaw>> handler)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.DealerToCallee,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            IEnumerable<WampMessage<MockRaw>> calls =
                GetCalls(scenario, Channel.CalleeToDealer, MessageTypes.Rpc).Concat
                    (GetCalls(scenario, Channel.DealerToCallee, MessageTypes.Rpc)).ToList();

            CalleeMessagePlayer player =
                new CalleeMessagePlayer(calls,
                                      new[] { WampMessageType.v2Invocation },
                                      handler);

            IMessageRecorder<MockRaw> recorder =
                new ResponsiveMessageRecorder(calls,
                                        new Dictionary<WampMessageType, string>()
                                            {
                                                {WampMessageType.v2Registered, "registrationId"}
                                            });

            IWampClientProxy<MockRaw> built =
                clientBuilder.Create(player,
                                     recorder,
                                     welcome);

            player.Client = built;

            MockClient<IWampClientProxy<MockRaw>> result =
                new MockClient<IWampClientProxy<MockRaw>>(built, recorder);

            return result;
        }
    }
}