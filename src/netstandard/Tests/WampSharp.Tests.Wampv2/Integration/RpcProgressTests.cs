using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
                    new CallOptions() { ReceiveProgress = true },
                    MyOperation.ProcedureUri,
                    new object[] { 10 });

            int? result = await callback.Task;

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), callback.ProgressiveResults);
            Assert.That(result, Is.EqualTo(10));
        }


        [Test]
        public async Task ProgressiveCallsCallerProgressObservable()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            var service = new LongOpObservableService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                    new CallOptions() { ReceiveProgress = true },
                    MyOperation.ProcedureUri,
                    new object[] { 10 });

            Assert.That(service.State, Is.EqualTo(LongOpObservableService.OperationState.Called));
            int? result = await callback.Task;
            Assert.That(result, Is.EqualTo(null));
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), callback.ProgressiveResults);
            Assert.That(service.State, Is.EqualTo(LongOpObservableService.OperationState.Completed));
        }

        [Test]
        public async Task ProgressiveCallsCallerProgressCancelObservable()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            var service = new LongOpObservableService();
            await calleeChannel.RealmProxy.Services.RegisterCallee(service);

            MyCallback callback = new MyCallback();

            var invocation = callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback,
                    new CallOptions() { ReceiveProgress = true },
                    MyOperation.ProcedureUri,
                    new object[] { 10 });

            Assert.That(service.State, Is.EqualTo(LongOpObservableService.OperationState.Called));
            invocation.Cancel(new CancelOptions());
            Assert.That(service.State, Is.EqualTo(LongOpObservableService.OperationState.Cancelled));
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

            string result = await proxy.LongOp(10, progress);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);

            Assert.That(result, Is.EqualTo("10"));
        }

        [Test]
        public async Task ProgressiveCallsCalleeProxyProgressValueTuples()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyValueTupleOperation myOperation = new MyValueTupleOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

            List<(int a, int b)> results = new List<(int a, int b)>();
            MyProgress<(int a, int b)> progress = new MyProgress<(int a, int b)>(i => results.Add(i));

            var result = await proxy.LongOpValueTuple(10, progress);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10).Select(x => (a:x,b:x)), results);

            Assert.That(result, Is.EqualTo((10, "10")));
        }


        [Test]
        public async Task ProgressiveCallsCalleeProxyProgressValueTuplesReflectionCallee()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            await calleeChannel.RealmProxy.Services.RegisterCallee(new LongOpService());
            ILongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

            List<(int a, int b)> results = new List<(int a, int b)>();
            MyProgress<(int a, int b)> progress = new MyProgress<(int a, int b)>(i => results.Add(i));

            var result = await proxy.LongOpValueTuple(10, progress);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10).Select(x => (a:x,b:x)), results);

            Assert.That(result, Is.EqualTo((10, "10")));
        }


        [Test]
        public async Task ProgressiveCallsCalleeProxyProgressTask()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            LongOpService myOperation = new LongOpService();

            await calleeChannel.RealmProxy.Services.RegisterCallee(myOperation);
            ILongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

            List<int> results = new List<int>();
            MyProgress<int> progress = new MyProgress<int>(i => results.Add(i));

            await proxy.LongOpTask(10, progress);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);
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
            ILongOpObservableService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpObservableService>();

            IObservable<int> proxyResult = proxy.LongOp(9); // it will emit one more than asked

            IEnumerable<int> results = proxyResult.ToEnumerable();

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);
        }

        [Test]
        public async Task ProgressiveCallsCalleeProxyObservableError()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation { EndWithError = true };

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpObservableService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpObservableService>();

            Assert.Throws(typeof(WampException), () => proxy.LongOp(9).ToEnumerable().Count());
        }

        public class MyOperation : IWampRpcOperation
        {
            public const string ProcedureUri = "com.myapp.longop";

            public string Procedure => ProcedureUri;

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

                for (int i = 0; i < n; i++)
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions { Progress = true },
                        new object[] { i });
                }

                if (EndWithError)
                {
                    caller.Error(WampObjectFormatter.Value,
                                 new Dictionary<string, string>(),
                                 "longop.error",
                                 new object[] { "Something bad happened" });
                }
                else
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions(),
                        new object[] { n });
                }

                return null;
            }

            public bool EndWithError { get; set; }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                                                               TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                return null;
            }
        }

        public class MyValueTupleOperation : IWampRpcOperation
        {
            public const string ProcedureUri = "com.myapp.longopvaluetuple";

            public string Procedure => ProcedureUri;

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

                for (int i = 0; i < n; i++)
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions { Progress = true },
                       new object[]{},
                        new Dictionary<string, object>
                        {
                            { "a", i },
                            { "b", i }
                        });
                }

                if (EndWithError)
                {
                    caller.Error(WampObjectFormatter.Value,
                                 new Dictionary<string, string>(),
                                 "longop.error",
                                 new object[] { "Something bad happened" });
                }
                else
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions(),
                        new object[] { n, n.ToString() });
                }

                return null;
            }

            public bool EndWithError { get; set; }

            public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                                                               TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                return null;
            }
        }

        public interface ILongOpService
        {
            [WampProcedure(MyOperation.ProcedureUri)]
            [WampProgressiveResultProcedure]
            Task<string> LongOp(int n, IProgress<int> progress);

            [WampProcedure(MyValueTupleOperation.ProcedureUri)]
            [WampProgressiveResultProcedure]
            Task<(int, string)> LongOpValueTuple(int n, IProgress<(int a, int b)> progress);

            [WampProcedure(MyOperation.ProcedureUri + "task")]
            [WampProgressiveResultProcedure]
            Task LongOpTask(int n, IProgress<int> progress);
        }

        public class LongOpService : ILongOpService
        {
            public async Task<string> LongOp(int n, IProgress<int> progress)
            {
                for (int i = 0; i < n; i++)
                {
                    progress.Report(i);
                    await Task.Delay(100);
                }

                return n.ToString();
            }

            public async Task<(int, string)> LongOpValueTuple(int n, IProgress<(int a, int b)> progress)
            {
                for (int i = 0; i < n; i++)
                {
                    progress.Report((i, i));
                    await Task.Delay(100);
                }

                return (n, n.ToString());
            }

            public async Task LongOpTask(int n, IProgress<int> progress)
            {
                for (int i = 0; i < n; i++)
                {
                    progress.Report(i);
                    await Task.Delay(100);
                }
            }
        }

        public interface ILongOpObservableService
        {
            [WampProcedure(MyOperation.ProcedureUri)]
            [WampProgressiveResultProcedure]
            IObservable<int> LongOp(int n);
        }

        public class LongOpObservableService : ILongOpObservableService
        {
            public enum OperationState
            {
                Nothing,
                Called,
                Completed,
                Cancelled
            }

            public OperationState State { get; set; } = OperationState.Nothing;

            public IObservable<int> LongOp(int n) => Observable.Create<int>(async (observer, cancellationToken) =>
            {
                State = OperationState.Called;

                cancellationToken.Register(() =>
                {
                    if (State == OperationState.Called)
                    {
                        State = OperationState.Cancelled;
                    }
                });

                for (int i = 0; i < n; i++)
                {
                    observer.OnNext(i);
                    await Task.Delay(100, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                State = OperationState.Completed;

                if (EndWithError)
                {
                    observer.OnError(new WampException("longop.error", "Something bad happened"));
                }
                else
                {
                    observer.OnCompleted();
                }
            });

            public bool EndWithError { get; set; }
        }

        public class MyCallback : IWampRawRpcOperationClientCallback
        {
            private readonly TaskCompletionSource<int?> mTask = new TaskCompletionSource<int?>();

            public List<int> ProgressiveResults { get; } = new List<int>();

            public Task<int?> Task => mTask.Task;

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
                mTask.SetResult(null);
                // null indicates no final return value
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
