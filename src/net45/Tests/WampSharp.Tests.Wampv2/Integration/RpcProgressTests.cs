#if !NET40
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task ProgressiveCallsCalleeProxyProgress()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await SetupService(playground);
            IWampChannel calleeChannel = dualChannel.CalleeChannel;
            IWampChannel callerChannel = dualChannel.CallerChannel;

            MyOperation myOperation = new MyOperation();

            await calleeChannel.RealmProxy.RpcCatalog.Register(myOperation, new RegisterOptions());
            ILongOpService proxy = callerChannel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

            List<int> results = new List<int>();
            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += (sender, i) => results.Add(i);
            Task<int> result = proxy.LongOp(10, progress);

            CollectionAssert.AreEquivalent(Enumerable.Range(0, 10), results);
            Assert.That(result.Result, Is.EqualTo(10));
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
                    return "com.myapp.longop";
                }
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
            {
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                InvocationDetails details,
                TMessage[] arguments)
            {
                TMessage number = arguments[0];
                int n = formatter.Deserialize<int>(number);

                for (int i = 0; i < n; i++)
                {
                    caller.Result(WampObjectFormatter.Value,
                        new YieldOptions {Progress = true},
                        new object[] {i});
                }

                caller.Result(WampObjectFormatter.Value,
                    new YieldOptions(),
                    new object[] {n});
            }

            public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
            }
        }

        public interface ILongOpService
        {
            [WampProcedure("com.myapp.longop")]
            [WampProgressiveResultProcedure]
            Task<int> LongOp(int n, IProgress<int> progress);
        }
    }
}
#endif