using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Dealer
{
    [TestFixture]
    public class DealerTests
    {
        class RegistrationTokenMock : IWampRegistrationSubscriptionToken
        {
            public void Dispose()
            {
            }

            public long TokenId { get; set; }
        }

        [Test]
        [TestCaseSource(nameof(GetRegistrationsTestCases))]
        public void RegisterCallsCatalogRegister(Registration[] registrations)
        {
            Mock<IWampRpcOperationCatalog> catalog = new Mock<IWampRpcOperationCatalog>();

            Mock<IWampCallee> callee = new Mock<IWampCallee>();
            callee.As<IWampConnectionMonitor>();

            WampRpcServer<MockRaw> server =
                new WampRpcServer<MockRaw>
                    (catalog.Object,
                     new MockBinding(), 
                     new LooseUriValidator());

            catalog.Setup(x => x.Register
                              (It.IsAny<IWampRpcOperation>(),
                               It.IsAny<RegisterOptions>()))
                   .Returns(new RegistrationTokenMock());

            foreach (Registration registration in registrations)
            {
                server.Register(callee.Object, registration.RequestId, registration.Options, registration.Procedure);

                catalog.Verify(x => x.Register
                                        (It.Is<IWampRpcOperation>
                                             (operation => operation.Procedure == registration.Procedure),
                                             It.IsAny<RegisterOptions>()),
                               Times.Exactly(1));

                callee.Verify(x => x.Registered(registration.RequestId, It.IsAny<long>()),
                              Times.Exactly(1));
            }
        }

        [Test]
        [TestCaseSource(nameof(GetRegistrationsTestCases))]
        public void RegisterCatalogErrorCallsCalleeError(Registration[] registrations)
        {
            Mock<IWampRpcOperationCatalog> catalog = new Mock<IWampRpcOperationCatalog>();

            string errorUri = "myerror";
            string argument = "any details";

            catalog.Setup(x => x.Register(It.IsAny<IWampRpcOperation>(),
                                          It.IsAny<RegisterOptions>()))
                   .Throws(new WampException(errorUri, argument));

            Mock<IWampCallee> callee = new Mock<IWampCallee>();
            callee.As<IWampConnectionMonitor>();

            WampRpcServer<MockRaw> server =
                new WampRpcServer<MockRaw>
                    (catalog.Object,
                     new MockBinding(),
                     new LooseUriValidator());

            foreach (Registration registration in registrations)
            {
                server.Register(callee.Object, registration.RequestId, registration.Options, registration.Procedure);

                callee.Verify(x => x.Error
                                       ((int) WampMessageType.v2Register,
                                        registration.RequestId,
                                        It.IsAny<object>(),
                                        errorUri,
                                        It.Is((object[] array) => array.SequenceEqual(new[] { argument }))),
                              Times.Exactly(1));
            }
        }

        [Test]
        [TestCaseSource(nameof(GetCallTestCases))]
        public void CallCallsCatalogInvoke(Call call)
        {
            Mock<IWampRpcOperationCatalog> catalog = new Mock<IWampRpcOperationCatalog>();

            Mock<IWampCaller> caller = new Mock<IWampCaller>();
            caller.As<IWampConnectionMonitor>();
            caller.As<IWampClientProxy>();
            caller.As<IWampClientProperties>()
                  .Setup(x => x.WelcomeDetails)
                  .Returns(new WelcomeDetails());

            WampRpcServer<MockRaw> server =
                new WampRpcServer<MockRaw>
                    (catalog.Object,
                     new MockBinding(),
                     new LooseUriValidator());

            if (call.Arguments == null)
            {
                server.Call(caller.Object, call.RequestId, call.Options, call.Procedure);

                catalog.Verify(x => x.Invoke(It.IsAny<IWampRawRpcOperationRouterCallback>(),
                                             It.IsAny<IWampFormatter<MockRaw>>(),
                                             It.IsAny<InvocationDetails>(),
                                             call.Procedure),
                               Times.Exactly(1));
            }
            else if (call.ArgumentsKeywords == null)
            {
                server.Call(caller.Object, call.RequestId, call.Options, call.Procedure, call.Arguments);

                catalog.Verify(x => x.Invoke(It.IsAny<IWampRawRpcOperationRouterCallback>(),
                                             It.IsAny<IWampFormatter<MockRaw>>(),
                                             It.IsAny<InvocationDetails>(),
                                             call.Procedure, 
                                             call.Arguments),
                               Times.Exactly(1));
            }
            else
            {
                server.Call(caller.Object, call.RequestId, call.Options, call.Procedure, call.Arguments, call.ArgumentsKeywords);

                catalog.Verify(x => x.Invoke(It.IsAny<IWampRawRpcOperationRouterCallback>(),
                                             It.IsAny<IWampFormatter<MockRaw>>(),
                                             It.IsAny<InvocationDetails>(),
                                             call.Procedure,
                                             call.Arguments,
                                             call.ArgumentsKeywords),
                               Times.Exactly(1));
            }
        }

        public static IEnumerable<TestCaseData> GetRegistrationsTestCases()
        {
            foreach (Type scenario in typeof (TestHelpers.Rpc).GetNestedTypes())
            {
                Type calleeToDealer = scenario.GetNestedType("CalleeToDealer");

                PropertyInfo callsProperty =
                    calleeToDealer.GetProperty("Calls", BindingFlags.Static |
                                                        BindingFlags.Public);

                IEnumerable<WampMessage<MockRaw>> calls =
                    callsProperty.GetValue(null, null) as IEnumerable<WampMessage<MockRaw>>;

                Registration[] registration =
                    calls.Where(x => x.MessageType == WampMessageType.v2Register)
                         .Select(x => new Registration(x)).ToArray();

                TestCaseData testCase = new TestCaseData((object)registration);

                testCase.SetName(scenario.Name);

                yield return testCase;
            }
        }

        public static IEnumerable<TestCaseData> GetCallTestCases()
        {
            foreach (Type scenario in typeof(TestHelpers.Rpc).GetNestedTypes())
            {
                Type calleeToDealer = scenario.GetNestedType("CallerToDealer");

                PropertyInfo callsProperty =
                    calleeToDealer.GetProperty("Calls", BindingFlags.Static |
                                                        BindingFlags.Public);

                IEnumerable<WampMessage<MockRaw>> calls =
                    callsProperty.GetValue(null, null) as IEnumerable<WampMessage<MockRaw>>;

                IEnumerable<Call> callsToCall =
                    calls.Where(x => x.MessageType == WampMessageType.v2Call)
                         .Select(x => new Call(x));

                foreach (Call call in callsToCall)
                {
                    TestCaseData testCase = new TestCaseData(call);

                    testCase.SetName(scenario.Name);

                    yield return testCase;                    
                }
            }
        }

    }
}