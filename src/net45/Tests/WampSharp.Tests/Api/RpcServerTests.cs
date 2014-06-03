using Moq;
using NUnit.Framework;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V1;
using WampSharp.V1.Rpc;

namespace WampSharp.Tests.Api
{
    [TestFixture]
    public class RpcServerTests
    {
        public interface ICalculator
        {
            [WampRpcMethod("test/square")]
            int Square(int x);
        }

        [Test]
        public void RequestContextIsSet()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;
            
            WampRequestContext context = null;

            Mock<ICalculator> calculatorMock = new Mock<ICalculator>();

            calculatorMock.Setup(x => x.Square(It.IsAny<int>()))
                          .Callback((int x) => context = WampRequestContext.Current)
                          .Returns((int x) => x*x);

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            ICalculator proxy = channel.GetRpcProxy<ICalculator>();

            int sixteen = proxy.Square(4);

            Assert.That(context.SessionId, Is.EqualTo(channel.GetMonitor().SessionId));
        }
    }
}