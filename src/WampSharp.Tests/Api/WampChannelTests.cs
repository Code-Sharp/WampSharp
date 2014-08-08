using System;
using NUnit.Framework;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Api
{
    [TestFixture]
    public class WampChannelTests
    {
        [Test]
        public void OpenAsyncWillNotBlockOnConnectionLost()
        {
            var wampChannelFactory = new WampChannelFactory<MockRaw>(new MockRawFormatter());
            var mockControlledWampConnection = new MockControlledWampConnection<MockRaw>();

            var wampChannel = wampChannelFactory.CreateChannel(mockControlledWampConnection);
            var openAsync = wampChannel.OpenAsync();

            Assert.IsFalse(openAsync.IsCompleted);
            mockControlledWampConnection.OnCompleted();
            Assert.IsTrue(openAsync.IsCompleted);
        }

        [Test]
        public void OpenAsyncWillThrowAndExpcetionIfAnErrorOccurs()
        {
            var wampChannelFactory = new WampChannelFactory<MockRaw>(new MockRawFormatter());
            var mockControlledWampConnection = new MockControlledWampConnection<MockRaw>();

            var wampChannel = wampChannelFactory.CreateChannel(mockControlledWampConnection);
            var openAsyncTask = wampChannel.OpenAsync();

            Exception myException = new Exception("My exception");

            mockControlledWampConnection.OnError(myException);

            Assert.IsNotNull(openAsyncTask.Exception);

            Assert.That(openAsyncTask.Exception.InnerException, Is.EqualTo(myException));
        }         
    }
}