using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    [TestFixture]
    internal class DealerIntegrationTests : IntegrationTestsBase
    {
        [TestCaseSource("TestCases")]
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

        
        public IEnumerable<TestCaseData> TestCases()
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

                    MockClient<IWampClient<MockRaw>> callee = GetCallee(nestedType, scenario.ClientBuilder, scenario.Handler);
                    MockClient<IWampClient<MockRaw>> caller = GetCaller(nestedType, scenario.ClientBuilder);

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
                    testCase.SetName(string.Format("DealerIntegrationTest.{0}.{1}",
                                                   nestedType.Name,
                                                   request.Arguments[2].Value));

                    if (nestedType.Name == "Options" || nestedType.Name == "Progress")
                    {
                        testCase.Ignore("WAMP2 Advanced profile feature.");
                    }

                    yield return testCase;
                }
            }
        }

        private static MockClient<IWampClient<MockRaw>> GetCaller(Type scenario, WampMockClientBuilder<MockRaw> builder)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.DealerToCallee,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            long sessionId = (long)welcome.Arguments[0].Value;

            NullPlayer<MockRaw> nullPlayer =
                new NullPlayer<MockRaw>();

            IMessageRecorder<MockRaw> messageRecorder =
                new MessageRecorder<MockRaw>();

            IWampClient<MockRaw> built =
                builder.Create(sessionId, nullPlayer,
                               messageRecorder);

            MockClient<IWampClient<MockRaw>> result =
                new MockClient<IWampClient<MockRaw>>(built, messageRecorder);

            return result;
        }

        private static MockClient<IWampClient<MockRaw>> GetCallee(Type scenario, WampMockClientBuilder<MockRaw> clientBuilder, IWampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> handler)
        {
            WampMessage<MockRaw> welcome =
                GetCalls(scenario, Channel.DealerToCallee,
                         new WampMessageType[] { WampMessageType.v2Welcome })
                    .FirstOrDefault();

            IEnumerable<WampMessage<MockRaw>> calls =
                GetCalls(scenario, Channel.CalleeToDealer, MessageTypes.Rpc).Concat
                    (GetCalls(scenario, Channel.DealerToCallee, MessageTypes.Rpc)).ToList();

            long sessionId = (long)welcome.Arguments[0].Value;

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

            IWampClient<MockRaw> built =
                clientBuilder.Create(sessionId, player,
                                      recorder);

            player.Client = built;

            MockClient<IWampClient<MockRaw>> result =
                new MockClient<IWampClient<MockRaw>>(built, recorder);

            return result;
        }
    }
}