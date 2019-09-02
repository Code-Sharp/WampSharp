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
        public async Task NoOptionsNoSessionId()
        {
            await RawTest(false, new RegisterOptions(), new CallOptions());
        }

        [Test]
        public async Task DiscloseOnRegisterOptionsDontDiscloseOnCallOptionsError()
        {
            await RawTest(false, new RegisterOptions() {DiscloseCaller = true}, new CallOptions() {DiscloseMe = false});
        }

        [Test]
        public async Task DontDiscloseOnRegisterOptionsDontDiscloseOnCallOptionsNoSession()
        {
            await RawTest(false, new RegisterOptions() { DiscloseCaller = false }, new CallOptions() { DiscloseMe = false });
        }
        [Test]
        public async Task DontMentionDiscloseRegisterOptionsDontDiscloseOnCallOptionsNoSession()
        {
            await RawTest(false, new RegisterOptions(), new CallOptions() { DiscloseMe = false });
        }

        [Test]
        public async Task DiscloseOnRegisterOptionsSessionId()
        {
            await RawTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions());
        }

        [Test]
        public async Task DiscloseMeOnCallOptionsSessionId()
        {
            await RawTest(true, new RegisterOptions(), new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async Task DiscloseMeOnCallOptionsAndDiscloseOnRegisterSessionId()
        {
            await RawTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async Task NoOptionsNoSessionIdMethodInfo()
        {
            await MethodInfoTest(false, new RegisterOptions(), new CallOptions());
        }

        [Test]
        public async Task DiscloseOnRegisterOptionsSessionIdMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions());
        }

        [Test]
        public async Task DiscloseMeOnCallOptionsSessionIdMethodInfoMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions(), new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async Task DiscloseMeOnCallOptionsAndDiscloseOnRegisterSessionIdMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions() { DiscloseMe = true });
        }

        [Test]
        public async Task DontDiscloseMeOnCallOptionsAndDiscloseOnRegisterErrorMethodInfo()
        {
            await MethodInfoTest(true, new RegisterOptions() { DiscloseCaller = true }, new CallOptions() { DiscloseMe = false });
        }

        [Test]
        public async Task DontDiscloseMeOnCallOptionsAndDontDiscloseOnRegisterNoSessionIdMethodInfo()
        {
            await MethodInfoTest(false, new RegisterOptions() { DiscloseCaller = false }, new CallOptions() { DiscloseMe = false });
        }

        [Test]
        public async Task DontDiscloseMeOnCallOptionsAndDontSpecifyDiscloseOnRegisterNoSessionIdMethodInfo()
        {
            await MethodInfoTest(false, new RegisterOptions(), new CallOptions() { DiscloseMe = false });
        }

        private static async Task RawTest(bool hasSessionId, RegisterOptions registerOptions, CallOptions callOptions)
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, registerOptions);
            MyCallback myCallback = new MyCallback();
            callerChannel.RealmProxy.RpcCatalog.Invoke(myCallback, callOptions, myOperation.Procedure);

            long? expectedCaller = null;

            if (hasSessionId)
            {
                expectedCaller = dualChannel.CallerSessionId;
            }

            if (callOptions.DiscloseMe == false && registerOptions.DiscloseCaller == true)
            {
                Assert.That(myCallback.ErrorUri, Is.EqualTo(WampErrors.DiscloseMeNotAllowed));
            }
            else
            {
                Assert.That(myOperation.Details.Caller, Is.EqualTo(expectedCaller));
            }
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
                callerChannel.RealmProxy.Services.GetCalleeProxy<IAddService>(new CalleeProxyInterceptor(callOptions));

            WampException caughtException = null;

            try
            {
                int seven = calleeProxy.Add2(3, 4);
            }
            catch (WampException ex)
            {
                caughtException = ex;
            }

            InvocationDetails details = service.Details;

            long? expectedCaller = null;

            if (hasSessionId)
            {
                expectedCaller = dualChannel.CallerSessionId;
            }

            if (registerOptions.DiscloseCaller == true && callOptions.DiscloseMe == false)
            {
                Assert.That(caughtException.ErrorUri, Is.EqualTo(WampErrors.DiscloseMeNotAllowed));
                Assert.That(details, Is.EqualTo(null));
            }
            else
            {
                Assert.That(details.Caller, Is.EqualTo(expectedCaller));
            }
        }

        public class MyOperation : IWampRpcOperation
        {
            public string Procedure => "my.operation";

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
            {
                this.Details = details;
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments)
            {
                this.Details = details;
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                this.Details = details;
                return null;
            }

            public InvocationDetails Details { get; set; }
        }

        class MyCallback : IWampRawRpcOperationClientCallback
        {

            public string ErrorUri { get; private set; }

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
                ErrorUri = error;
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
            {
                ErrorUri = error;
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                TMessage argumentsKeywords)
            {
                ErrorUri = error;
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