using System.Reactive.Concurrency;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using WampSharp.V2.Transports;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class GoodbyeTests
    {
        [Test]
        public async Task InMemoryTest()
        {
            var transport = new InMemoryTransport(Scheduler.Default);
            var binding = new JTokenJsonBinding();
            var realm = "some.realm";

            var router = new WampHost();
            router.RegisterTransport(transport, new[] { binding });
            router.Open();

            var calleeConnection = transport.CreateClientConnection(binding, Scheduler.Default);
            WampChannelFactory factory = new WampChannelFactory();

            var callee = factory.CreateChannel(realm, calleeConnection, binding);
            await callee.Open();
            await callee.RealmProxy.Services.RegisterCallee(new WampTest());


            var callerConnection = transport.CreateClientConnection(binding, Scheduler.Default);
            var caller = factory.CreateChannel(realm, callerConnection, binding);
            await caller.Open();

            var proxy = caller.RealmProxy.Services.GetCalleeProxy<IWampTest>();
            var result = await proxy.Echo("1");
            Assert.That(result, Is.EqualTo("1"));

            await caller.Close(WampErrors.CloseNormal, new GoodbyeDetails());
            await callee.Close(WampErrors.CloseNormal, new GoodbyeDetails());

            router.Dispose();
        }

        public interface IWampTest
        {
            [WampProcedure("com.test.echo")]
            Task<string> Echo(string message);
        }

        public class WampTest : IWampTest
        {
            public Task<string> Echo(string message)
            {
                return Task.FromResult(message);
            }
        }

    }
}