using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class MockOutgoingMessageHandlerBuilder : IWampOutgoingMessageHandlerBuilder<MockRaw>
    {
        private readonly MockOutgoingMessageHandler mHandler;

        public MockOutgoingMessageHandlerBuilder(MockOutgoingMessageHandler handler)
        {
            mHandler = handler;
        }

        public IWampOutgoingMessageHandler Build(IWampConnection<MockRaw> connection)
        {
            return mHandler;
        }
    }
}