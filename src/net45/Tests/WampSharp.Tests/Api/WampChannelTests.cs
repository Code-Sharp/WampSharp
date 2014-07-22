using NUnit.Framework;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1;

namespace WampSharp.Tests.Api
{
    public class WampChannelTests
    {
        [Test]
        public void OpenWillNotBlockOnConnectionLost()
        {
            var wampChannelFactory = new WampChannelFactory<MockRaw>(new MockRawFormatter());
            var mockConnection = new MockConnection<MockRaw>();
            var mockControlledWampConnection = mockConnection.SideAToSideB;
            
            var wampChannel = wampChannelFactory.CreateChannel(mockControlledWampConnection);
            var openAsync = wampChannel.OpenAsync();

            Assert.IsFalse(openAsync.IsCompleted);
            mockConnection.SideBToSideA.Dispose();
            Assert.IsTrue(openAsync.IsCompleted);
        }
    }
}