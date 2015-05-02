#if !NET40
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class RpcOptionsTests
    {
        [Test]
        public async void NoOptionsNoSessionId()
        {
            await RawTest(false, new RegisterOptions(), new CallOptions());
        }

        [Test]
        public async void DiscloseOnRegisterOptionsSessionId()
        {
            await RawTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions());
        }

        [Test]
        public async void DiscloseMeOnCallOptionsSessionId()
        {
            await RawTest(true, new RegisterOptions(), new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async void DiscloseMeOnCallOptionsAndDiscloseOnRegisterSessionId()
        {
            await RawTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async void NoOptionsNoSessionIdMethodInfo()
        {
            await MethodInfoTest(false, new RegisterOptions(), new CallOptions());
        }

        [Test]
        public async void DiscloseOnRegisterOptionsSessionIdMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions());
        }

        [Test]
        public async void DiscloseMeOnCallOptionsSessionIdMethodInfoMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions(), new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async void DiscloseMeOnCallOptionsAndDiscloseOnRegisterSessionIdMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions() { DiscloseMe = true });
        }

        private static async Task RawTest(bool hasSessionId, RegisterOptions registerOptions, CallOptions callOptions)
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
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

        private async Task MethodInfoTest(bool hasSessionId, RegisterOptions registerOptions, CallOptions callOptions)
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyService service = new MyService();

            Task registerTask =
                calleeChannel.RealmProxy.Services.RegisterCallee(service,
                    new CalleeRegistrationInterceptor(registerOptions));

            await registerTask;

            IAddService calleeProxy =
                callerChannel.RealmProxy.Services.GetCalleeProxyPortable<IAddService>(new CalleeProxyInterceptor(callOptions));

            int seven = calleeProxy.Add2(3, 4);

            InvocationDetails details = service.Details;

            long? expectedCaller = null;

            if (hasSessionId)
            {
                expectedCaller = dualChannel.CallerSessionId;
            }

            Assert.That(details.Caller, Is.EqualTo(expectedCaller));
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

        class MyService
        {
            public InvocationDetails Details { get; private set; }

            [WampProcedure("com.arguments.add2")]
            public int Add2(int x, int y)
            {
                Details = WampInvocationContext.Current.InvocationDetails;
                return (x + y);
            }
        }

        public interface IAddService
        {
            [WampProcedure("com.arguments.add2")]
            int Add2(int x, int y);
        }
    }
}
#endif