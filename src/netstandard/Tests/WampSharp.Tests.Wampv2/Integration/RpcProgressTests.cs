using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class RpcProgressTests
    {
        [Test]
        public async Task ProgressiveCallsCallerProgress()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            await calleeChannel.RealmProxy.Services.RegisterCallee(new LongOpService());

            MyCallback callback = new MyCallback();

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                    new CallOptions() {ReceiveProgress = true},
                    "com.myapp.longop",
                    new object[] {10});

            callback.Task.Wait(2000);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), callback.ProgressiveResults);
            Assert.That(callback.Task.Result, Is.EqualTo(10));
        }

        [Test]
        public async Task ProgressiveCallsCallerProgressObservable()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            var service = new LongOpObsService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                    new CallOptions() { ReceiveProgress = true },
                    "com.myapp.longop",
                    new object[] { 10, false });

            Assert.That(service.State, Is.EqualTo(LongOpObsService.EState.Called));
            Assert.That(callback.Task.Result, Is.EqualTo(-1));
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), callback.ProgressiveResults);
            Assert.That(service.State, Is.EqualTo(LongOpObsService.EState.Completed));
        }

        [Test]
        public async Task ProgressiveCallsCallerProgressCancelObservable()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            var service = new LongOpObsService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            var invocation = callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                    new CallOptions() { ReceiveProgress = true },
                    "com.myapp.longop",
                    new object[] { 10, false });

            Assert.That(service.State, Is.EqualTo(LongOpObsService.EState.Called));
            invocation.Cancel(new CancelOptions());
            Assert.That(service.State, Is.EqualTo(LongOpObsService.EState.Cancelled));
        }

        [Test]
        public async Task ProgressiveCallsCalleeProxyProgress()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

            List<int> results = new List<int>();
            MyProgress<int> progress = new MyProgress<int>(i => results.Add(i));

            Task<int> result = proxy.LongOp(10, progress);
            result.Wait();

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);

            Assert.That(result.Result, Is.EqualTo(10));
        }

        [Test]
        public async Task ProgressiveCallsCalleeProxyObservable()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpObsService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpObsService>();

            IEnumerable<int> results = proxy.LongOp(9, false).ToEnumerable(); // it will emit one more than asked

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);
        }

        [Test]
        public async Task ProgressiveCallsCalleeProxyObservableError()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpObsService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpObsService>();

            Assert.Throws(typeof(WampException), () => proxy.LongOp(9, true).ToEnumerable().Count());
        }

        public class MyOperation : IWampRpcOperation
        {
            public string Procedure => "com.myapp.longop";

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
            {
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                InvocationDetails details,
                TMessage[] arguments)
            {
                TMessage number = arguments[0];
                int n = formatter.Deserialize<int>(number);

                bool endWithError = arguments.Length > 1 && formatter.Deserialize<bool>(arguments[1]);

                for (int i = 0; i < n; i++)
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions {Progress = true},
                        new object[] {i});
                }

                if (endWithError)
                {
                    caller.Error(WampObjectFormatter.Value,
                        new Dictionary<string, string>(),
                        "Something bad happened");
                }
                else
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions(),
                        new object[] { n });
                }

                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                return null;
            }
        }

        public interface ILongOpService
        {
            [WampProcedure("com.myapp.longop")]
            [WampProgressiveResultProcedure]
            Task<int> LongOp(int n, IProgress<int> progress);
        }

        public class LongOpService : ILongOpService
        {
            public async Task<int> LongOp(int n, IProgress<int> progress)
            {
                for (int i = 0; i < n; i++)
                {
                    progress.Report(i);
                    await Task.Delay(100);
                }

                return n;
            }
        }

        public interface ILongOpObsService
        {
            [WampProcedure("com.myapp.longop")]
            [WampProgressiveResultProcedure]
            IObservable<int> LongOp(int n, bool endWithError);
        }

        public class LongOpObsService : ILongOpObsService
        {
            public enum EState
            {
                Nothing,
                Called,
                Completed,
                Cancelled
            }

            public EState State { get; set; } = EState.Nothing;

            public IObservable<int> LongOp(int n, bool endWithError) => Observable.Create<int>(async (obs, ct) =>
            {
                State = EState.Called;
                ct.Register(() =>
                {
                    if (State == EState.Called)
                        State = EState.Cancelled;
                });
                for (int i = 0; i < n; i++)
                {
                    obs.OnNext(i);
                    await Task.Delay(100, ct);
                    ct.ThrowIfCancellationRequested();
                }
                State = EState.Completed;
                if (endWithError)
                    obs.OnError(new WampException("wamp.error", "Something bad happened"));
                else
                    obs.OnCompleted();
            });
        }

        public class MyCallback : IWampRawRpcOperationClientCallback
        {
            private readonly TaskCompletionSource<int> mTask = new TaskCompletionSource<int>();

            public List<int> ProgressiveResults { get; } = new List<int>();

            public Task<int> Task => mTask.Task;

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
                mTask.SetResult(-1); // -1 indicates no final return value
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
            {
                int current = formatter.Deserialize<int>(arguments[0]);

                if (details.Progress == true)
                {
                    ProgressiveResults.Add(current);
                }
                else
                {
                    mTask.SetResult(current);
                }
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
    }

    internal class MyProgress<T> : IProgress<T>
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
}
