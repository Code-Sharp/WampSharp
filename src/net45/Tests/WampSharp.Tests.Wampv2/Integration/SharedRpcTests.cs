using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemEx;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class SharedRpcTests
    {
        [Test]
        public async Task FirstPolicyCallsFirstCallee()
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            IPingService pingService = await GetCaller(playground);

            List<int> calls = new List<int>();

            List<Registration> registrations = 
                await RegisterCallees(playground, calls, 10, "first");

            await pingService.PingAsync();
            await registrations[0].DisposeAsync();
            await pingService.PingAsync();
            await pingService.PingAsync();
            await registrations[1].DisposeAsync();
            await pingService.PingAsync();
            await registrations[2].DisposeAsync();
            await pingService.PingAsync();
            await registrations[3].DisposeAsync();
            await registrations[4].DisposeAsync();
            await pingService.PingAsync();
            await registrations[5].DisposeAsync();
            await registrations[6].DisposeAsync();
            await registrations[7].DisposeAsync();
            await registrations[9].DisposeAsync();
            await pingService.PingAsync();

            CollectionAssert.AreEquivalent(new[] {0, 1, 1, 2, 3, 5, 8}, calls);
        }

        [Test]
        public async Task LastPolicyCallsLastCallee()
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            IPingService pingService = await GetCaller(playground);

            List<int> calls = new List<int>();

            List<Registration> registrations =
                await RegisterCallees(playground, calls, 10, "last");

            await pingService.PingAsync();
            await registrations[0].DisposeAsync();
            await pingService.PingAsync();
            await pingService.PingAsync();
            await registrations[9].DisposeAsync();
            await pingService.PingAsync();
            await registrations[1].DisposeAsync();
            await pingService.PingAsync();
            await registrations[8].DisposeAsync();
            await registrations[2].DisposeAsync();
            await pingService.PingAsync();
            await registrations[7].DisposeAsync();
            await registrations[3].DisposeAsync();
            await registrations[6].DisposeAsync();
            await registrations[4].DisposeAsync();
            await pingService.PingAsync();

            CollectionAssert.AreEquivalent(new[] { 9, 9,9, 8, 8, 7, 5 }, calls);
        }

        [Test]
        public async Task RoundRobinPolicy()
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            IPingService pingService = await GetCaller(playground);

            List<int> calls = new List<int>();

            List<Registration> registrations =
                await RegisterCallees(playground, calls, 10, "roundrobin");

            for (int i = 0; i < 15; i++)
            {
                await pingService.PingAsync();                
            }

            await registrations[0].DisposeAsync();
            await registrations[7].DisposeAsync();

            for (int i = 0; i < 15; i++)
            {
                await pingService.PingAsync();
            }

            List<int> modifiedIndexes = 
                Enumerable.Range(0, 10).Where(x => x != 0 && x != 7).ToList();

            IEnumerable<int> expected =
                Enumerable.Range(0, 15)
                          .Select(x => x%10)
                          .Concat(Enumerable.Range(15 % 10, 15)
                                            .Select(x => x%8)
                                            .Select(x => modifiedIndexes[x]))
                          .ToList();

            CollectionAssert.AreEquivalent(expected, calls);
        }

        private static async Task<IPingService> GetCaller(WampPlayground playground)
        {
            IWampChannel channel = playground.CreateNewChannel("realm1");

            await channel.Open();

            IPingService pingService =
                channel.RealmProxy.Services.GetCalleeProxyPortable<IPingService>();

            return pingService;
        }

        private static async Task<List<Registration>> RegisterCallees(WampPlayground playground, List<int> calls, int numberOfCallees, string invocationPolicy)
        {
            List<Registration> registrations = new List<Registration>();

            for (int i = 0; i < numberOfCallees; i++)
            {
                int currentIndex = i;

                Registration calleeRegistration =
                    await RegisterCallee
                        (playground,
                         registration => { calls.Add(currentIndex); },
                         invocationPolicy);

                registrations.Add(calleeRegistration);
            }

            return registrations;
        }

        private static async Task<Registration> RegisterCallee
            (WampPlayground playground,
             Action<Registration> action,
             string invocationPolicy)
        {
            IWampChannel calleeChannel = playground.CreateNewChannel("realm1");

            await calleeChannel.Open();

            Registration registration = null;

            MyOperation operation = new MyOperation(() => action(registration));

            IAsyncDisposable disposable =
                await calleeChannel.RealmProxy.RpcCatalog.Register
                    (operation,
                     new RegisterOptions()
                     {
                         Invoke = invocationPolicy
                     });

            registration = new Registration(operation, disposable);

            return registration;
        }

        private class MyOperation : LocalRpcOperation
        {
            private readonly Action mAction;

            public MyOperation(Action action)
                : base("com.arguments.ping")
            {
                mAction = action;
            }

            public override RpcParameter[] Parameters => new RpcParameter[0];

            public override bool HasResult => false;

            public override bool SupportsCancellation => false;

            public override CollectionResultTreatment CollectionResultTreatment => CollectionResultTreatment.SingleValue;

            protected override IWampCancellableInvocation InnerInvoke<TMessage>
                (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                mAction();
                caller.Result(formatter, new YieldOptions());
                return null;
            }
        }

        private class Registration : IAsyncDisposable
        {
            private readonly IWampRpcOperation mOperation;
            private readonly IAsyncDisposable mDisposable;

            public Registration(IWampRpcOperation operation, IAsyncDisposable disposable)
            {
                mOperation = operation;
                mDisposable = disposable;
            }

            public ValueTask DisposeAsync()
            {
                return mDisposable.DisposeAsync();
            }
        }
    
        public interface IPingService
        {
            [WampProcedure("com.arguments.ping")]
            Task PingAsync();
        }
    }
}
