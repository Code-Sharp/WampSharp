using System;
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
    public class RpcClientTests
    {
        public interface IAsyncCalculator
        {
            [WampRpcMethod("test/add")]
            Task<int> Add(int x, int y);
        }

        public interface ICalculator
        {
            [WampRpcMethod("test/add")]
            int Add(int x, int y);
        }

        private static Mock<IAsyncCalculator> GetAsyncErrorCalculatorMock(WampRpcCallException exception)
        {
            Mock<IAsyncCalculator> calculatorMock = new Mock<IAsyncCalculator>();

            TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
            completionSource.SetException(exception);

            calculatorMock.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
                          .Returns(completionSource.Task);

            return calculatorMock;
        }

        private static Mock<ICalculator> GetErrorCalculatorMock(WampRpcCallException exception)
        {
            Mock<ICalculator> calculatorMock = new Mock<ICalculator>();

            calculatorMock.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
                          .Throws(exception);

            return calculatorMock;
        }

        private static Mock<ICalculator> GetCalculatorMock()
        {
            Mock<ICalculator> calculatorMock = new Mock<ICalculator>();

            calculatorMock.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
                          .Returns((int x, int y) => x + y);

            return calculatorMock;
        }

        [Test]
        public void SyncClientRpcCallsServer()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            Mock<ICalculator> calculatorMock = GetCalculatorMock();

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            ICalculator proxy = channel.GetRpcProxy<ICalculator>();

            int seven = proxy.Add(3, 4);

            calculatorMock.Verify(x => x.Add(3, 4));

            Assert.That(seven, Is.EqualTo(7));
        }

        [Test]
        public void AsyncClientRpcCallsServer()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            Mock<ICalculator> calculatorMock = GetCalculatorMock();

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            IAsyncCalculator proxy = channel.GetRpcProxy<IAsyncCalculator>();

            Task<int> sevenTask = proxy.Add(3, 4);

            int seven = sevenTask.Result;

            calculatorMock.Verify(x => x.Add(3, 4));

            Assert.That(seven, Is.EqualTo(7));
        }

        [Test]
        public void SyncClientRpcCallsServerThrowsException()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            WampRpcCallException exception =
                new WampRpcCallException("calculator.add",
                                         "This is very bad caclulator implementation",
                                         null);
            
            Mock<ICalculator> calculatorMock = GetErrorCalculatorMock(exception);

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            ICalculator proxy = channel.GetRpcProxy<ICalculator>();

            WampRpcCallException thrown = 
                Assert.Throws<WampRpcCallException>(() => proxy.Add(3, 4));

            Assert.That(thrown.ProcUri, Is.EqualTo("test/add"));
            Assert.That(thrown.ErrorDetails, Is.EqualTo(exception.ErrorDetails));
            Assert.That(thrown.ErrorUri, Is.EqualTo(exception.ErrorUri));
            Assert.That(thrown.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public void SyncClientRpcCallsAsyncServerThrowsException()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            WampRpcCallException exception =
                new WampRpcCallException("calculator.add",
                                         "This is very bad caclulator implementation",
                                         null);

            Mock<IAsyncCalculator> calculatorMock = GetAsyncErrorCalculatorMock(exception);

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            ICalculator proxy = channel.GetRpcProxy<ICalculator>();

            WampRpcCallException thrown =
                Assert.Throws<WampRpcCallException>(() => proxy.Add(3, 4));

            Assert.That(thrown.ProcUri, Is.EqualTo("test/add"));
            Assert.That(thrown.ErrorDetails, Is.EqualTo(exception.ErrorDetails));
            Assert.That(thrown.ErrorUri, Is.EqualTo(exception.ErrorUri));
            Assert.That(thrown.Message, Is.EqualTo(exception.Message));
        }


        [Test]
        public void AsyncClientRpcCallsServerThrowsException()
        {
            WampPlayground playground = new WampPlayground();

            IWampHost host = playground.Host;

            WampRpcCallException exception =
                new WampRpcCallException("calculator.add",
                                         "This is very bad caclulator implementation",
                                         null);

            Mock<ICalculator> calculatorMock = GetErrorCalculatorMock(exception);

            host.HostService(calculatorMock.Object);

            host.Open();

            IWampChannel<MockRaw> channel = playground.CreateNewChannel();

            channel.Open();

            IAsyncCalculator proxy = channel.GetRpcProxy<IAsyncCalculator>();

            Task<int> task = proxy.Add(3, 4);

            try
            {
                task.Wait();
            }
            catch (Exception)
            {
            }

            AggregateException aggregateException = task.Exception;
            Assert.That(aggregateException, Is.Not.Null);

            Exception innerException = aggregateException.InnerException;

            Assert.That(innerException, Is.InstanceOf<WampRpcCallException>());

            WampRpcCallException thrown = innerException as WampRpcCallException;

            Assert.That(thrown.ProcUri, Is.EqualTo("test/add"));
            Assert.That(thrown.ErrorDetails, Is.EqualTo(exception.ErrorDetails));
            Assert.That(thrown.ErrorUri, Is.EqualTo(exception.ErrorUri));
            Assert.That(thrown.Message, Is.EqualTo(exception.Message));
        }    
    }
}