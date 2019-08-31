using WampSharp.Core.Listener;

namespace WampSharp.Tests.TestHelpers
{
    public static class DummyConnection<TMessage>
    {
        private static readonly MockConnection<TMessage> mMockConnection = new MockConnection<TMessage>(null);

        public static IWampConnection<TMessage> Instance => mMockConnection.SideAToSideB;
    }
}