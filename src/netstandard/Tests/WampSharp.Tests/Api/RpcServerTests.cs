using System.Threading.Tasks;
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

        public interface INumberProcessor
        {
            [WampRpcMethod("test/square")]
            Task ProcessNumber(int x);
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

        [Test]
        public void AsyncAwaitTaskWork()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            Mock<INumberProcessor> mock = new Mock<INumberProcessor>();

            mock.Setup(x => x.ProcessNumber(It.IsAny<int>()))
                          .Returns(async (int x) => { await Task.CompletedTask; });

            host.HostService(mock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            INumberProcessor proxy = channel.GetRpcProxy<INumberProcessor>();

            Task task = proxy.ProcessNumber(4);

            mock.Verify(x => x.ProcessNumber(4));
        }
    }
}