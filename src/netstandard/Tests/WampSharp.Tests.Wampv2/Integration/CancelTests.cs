using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class CancelTests
    {
        [Test]
        public async Task CancelProgressiveCallsCalleeCancellationToken()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            CancellableService service = new CancellableService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            IWampCancellableInvocationProxy cancellable =
                callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new CallOptions() { ReceiveProgress = true },
                 "com.myapp.longop",
                 new object[] { 100 });

            Assert.That(service.CancellationToken, Is.Not.Null);
            Assert.That(service.CancellationToken.IsCancellationRequested, Is.False);

            cancellable.Cancel(new CancelOptions());

            Assert.That(service.CancellationToken.IsCancellationRequested, Is.True);
        }

        [Test]
        public async Task ProgressiveCancellationTokenCancelCallsInterrupt()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyCancellableOperation myOperation = new MyCancellableOperation("com.myapp.longop");

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ICancellableLongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ICancellableLongOpService>();

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            MyProgress<int> progress = new MyProgress<int>(x => { });

            Task<int> result = proxy.LongOp(10, progress, tokenSource.Token);
            Assert.That(myOperation.CancellableInvocation.InterruptCalled, Is.False);

            tokenSource.Cancel();

            Assert.That(myOperation.CancellableInvocation.InterruptCalled, Is.True);
        }

        [Test]
        public async Task CancelCallsCalleeCancellationToken()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            CancellableService service = new CancellableService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            IWampCancellableInvocationProxy cancellable =
                callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new CallOptions() { ReceiveProgress = true },
                 "com.myapp.cancellable",
                 new object[] { 100 });

            Assert.That(service.CancellationToken, Is.Not.Null);
            Assert.That(service.CancellationToken.IsCancellationRequested, Is.False);

            cancellable.Cancel(new CancelOptions());

            Assert.That(service.CancellationToken.IsCancellationRequested, Is.True);
        }

        [Test]
        public async Task CancellationTokenCancelCallsInterrupt()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyCancellableOperation myOperation = new MyCancellableOperation("com.myapp.cancellable");

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ICancellableLongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ICancellableLongOpService>();

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Task<int> result = proxy.Cancellable(10, tokenSource.Token);
            Assert.That(myOperation.CancellableInvocation.InterruptCalled, Is.False);

            tokenSource.Cancel();

            Assert.That(myOperation.CancellableInvocation.InterruptCalled, Is.True);
        }

        private class MyCancellableOperation : IWampRpcOperation
        {
            public MyCancellableOperation(string procedure)
            {
                Procedure = procedure;
            }

            public MyCancellableInvocation CancellableInvocation { get; private set; }

            public string Procedure { get; }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
            {
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                                               InvocationDetails details,
                                                               TMessage[] arguments)
            {
                CancellableInvocation = new MyCancellableInvocation();
                return CancellableInvocation;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                                                               TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                return null;
            }
        }

        public class MyCallback : IWampRawRpcOperationClientCallback
        {
            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
                throw new NotImplementedException();
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
            {
                throw new NotImplementedException();
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments,
                                         IDictionary<string, TMessage> argumentsKeywords)
            {
                throw new NotImplementedException();
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
                throw new NotImplementedException();
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
            {
                throw new NotImplementedException();
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                                        TMessage argumentsKeywords)
            {
                throw new NotImplementedException();
            }
        }

        public class CancellableService
        {
            public CancellationToken CancellationToken { get; private set; }

            [WampProcedure("com.myapp.longop")]
            [WampProgressiveResultProcedure]
            public Task<int> LongOp(int n, IProgress<int> progress, CancellationToken cancellationToken)
            {
                CancellationToken = cancellationToken;

                return new TaskCompletionSource<int>().Task;
            }

            [WampProcedure("com.myapp.cancellable")]
            public Task<int> Cancellable(int n, CancellationToken cancellationToken)
            {
                CancellationToken = cancellationToken;

                TaskCompletionSource<int> result = new TaskCompletionSource<int>();
                return result.Task;
            }
        }

        public interface ICancellableLongOpService
        {
            [WampProcedure("com.myapp.longop")]
            [WampProgressiveResultProcedure]
            Task<int> LongOp(int n, IProgress<int> progress, CancellationToken cancellationToken);

            [WampProcedure("com.myapp.cancellable")]
            Task<int> Cancellable(int n, CancellationToken cancellationToken);
        }

        private class MyProgress<T> : IProgress<T>
        {
            private readonly Action<T> mAction;

            public MyProgress(Action<T> action)
            {
                mAction = action;
            }

            public void Report(T value)
            {
                mAction(value);
            }
        }

        private class MyCancellableInvocation : IWampCancellableInvocation
        {
            public void Cancel(InterruptDetails details)
            {
                InterruptCalled = true;
            }

            public bool InterruptCalled { get; set; }
        }
    }
}