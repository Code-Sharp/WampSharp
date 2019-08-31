using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Newtonsoft;
using WampSharp.Tests.Cra.Helpers;
using WampSharp.V1;
using WampSharp.V1.Cra;
using WampSharp.V1.Rpc;

namespace WampSharp.Tests.Cra
{
    [TestFixture]
    public class CraTests
    {
        public interface ISample
        {
            [WampRpcMethod("http://example.com/procedures/#hello")]
            string Hello(string name);
        }

        public interface ISampleAsync
        {
            [WampRpcMethod("http://example.com/procedures/#hello")]
            Task<string> Hello(string name);
        }


        private class Sample : ISample
        {
            public string Hello(string name)
            {
                return "Hello " + name;
            }
        }

        [Test]
        public void AuthenticationSuccess()
        {
            JsonFormatter formatter = new JsonFormatter();
            WampCraPlayground<JToken> playground = new WampCraPlayground<JToken>
                (formatter, new MockWampCraAuthenticaticationBuilder<JToken>());

            IWampChannelFactory<JToken> channelFactory = playground.ChannelFactory;

            IWampHost host = playground.Host;

            host.HostService(new Sample());

            host.Open();

            IControlledWampConnection<JToken> connection = playground.CreateClientConnection();

            IWampChannel<JToken> channel = channelFactory.CreateChannel(connection);

            channel.Open();

            channel.GetRpcProxy<IWampCraProcedures>().
                    Authenticate(formatter, "foobar", null, "secret");

            string result = channel.GetRpcProxy<ISample>()
                                   .Hello("Foobar");

            Assert.That(result, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void AuthenticationFailure()
        {
            JsonFormatter formatter = new JsonFormatter();
            WampCraPlayground<JToken> playground = new WampCraPlayground<JToken>
                (formatter, new MockWampCraAuthenticaticationBuilder<JToken>());

            IWampChannelFactory<JToken> channelFactory = playground.ChannelFactory;

            IWampHost host = playground.Host;

            host.HostService(new Sample());

            host.Open();

            IControlledWampConnection<JToken> connection = playground.CreateClientConnection();

            IWampChannel<JToken> channel = channelFactory.CreateChannel(connection);

            channel.Open();

            IWampCraProcedures proceduresProxy = channel.GetRpcProxy<IWampCraProcedures>();

            WampRpcCallException callException =
                Assert.Throws<WampRpcCallException>
                    (() => proceduresProxy.Authenticate(formatter, "foobar", null, "secret2"));
        }


        [Test]
        public void NoAuthenticationThrowsException()
        {
            JsonFormatter formatter = new JsonFormatter();
            WampCraPlayground<JToken> playground = new WampCraPlayground<JToken>
                (formatter, new MockWampCraAuthenticaticationBuilder<JToken>());

            IWampChannelFactory<JToken> channelFactory = playground.ChannelFactory;

            IWampHost host = playground.Host;

            host.HostService(new Sample());

            host.Open();

            IControlledWampConnection<JToken> connection = playground.CreateClientConnection();

            IWampChannel<JToken> channel = channelFactory.CreateChannel(connection);

            channel.Open();

            Task<string> result =
                channel.GetRpcProxy<ISampleAsync>()
                   .Hello("Foobar");

            AggregateException aggregateException = result.Exception;
            Exception exception = aggregateException.InnerException;
            WampRpcCallException casted = exception as WampRpcCallException;
            
            Assert.IsNotNull(casted);
        }
    
    }
}