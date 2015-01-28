using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.Integration.RpcProxies;
using WampSharp.Tests.Wampv2.Integration.RpcServices;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class RpcOptionsTests
    {
        [Test]
        public async void NoOptionsNoSessionId()
        {
            await GeneralTest(false, new RegisterOptions(), new CallOptions());
        }

        [Test]
        public async void DiscloseOnRegisterOptionsSessionId()
        {
            await GeneralTest(true, new RegisterOptions(){DiscloseCaller = true}, new CallOptions());
        }

        [Test]
        public async void DiscloseMeOnCallOptionsSessionId()
        {
            await GeneralTest(true, new RegisterOptions(), new CallOptions(){DiscloseMe = true});
        }

        [Test]
        public async void DiscloseMeOnCallOptionsAndDiscloseOnRegisterSessionId()
        {
            await GeneralTest(true, new RegisterOptions(){DiscloseCaller = true}, new CallOptions() { DiscloseMe = true });
        }

        private static async Task GeneralTest(bool hasSessionId, RegisterOptions registerOptions, CallOptions callOptions)
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await SetupService(playground);
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, registerOptions);
            callerChannel.RealmProxy.RpcCatalog.Invoke(new MyCallback(), callOptions, myOperation.Procedure);

            long? expectedCaller = null;

            if (hasSessionId)
            {
                expectedCaller = dualChannel.CallerSessionId;
            }

            Assert.That(myOperation.Details.Caller, Is.EqualTo(expectedCaller));
        }

        private static async Task<CallerCallee> SetupService(WampPlayground playground)
        {
            const string realmName = "realm1";

            playground.Host.Open();

            CallerCallee result = new CallerCallee();

            result.CalleeChannel =
                playground.CreateNewChannel(realmName);

            result.CallerChannel =
                playground.CreateNewChannel(realmName);

            long? callerSessionId = null;

            result.CallerChannel.RealmProxy.Monitor.ConnectionEstablished +=
                (x, y) => { callerSessionId = y.SessionId; };

            await result.CalleeChannel.Open();
            await result.CallerChannel.Open();

            result.CallerSessionId = callerSessionId.Value;

            return result;
        }

        private class CallerCallee
        {
            public IWampChannel CalleeChannel { get; set; }
            public IWampChannel CallerChannel { get; set; }
            public long CallerSessionId { get; set; }
        }

        public class MyOperation : IWampRpcOperation
        {
            public string Procedure
            {
                get
                {
                    return "my.operation";
                }
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
            {
                this.Details = details;
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments)
            {
                this.Details = details;
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                this.Details = details;
            }

            public InvocationDetails Details { get; set; }
        }

        class MyCallback : IWampRawRpcOperationClientCallback
        {
            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
            {
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords)
            {
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
            {
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                TMessage argumentsKeywords)
            {
            }
        }
    }
}