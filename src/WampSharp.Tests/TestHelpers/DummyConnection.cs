using WampSharp.Core.Listener;

namespace WampSharp.Tests.TestHelpers
{
    public static class DummyConnection<TMessage>
    {
        private static readonly MockConnection<TMessage> mMockConnection = new MockConnection<TMessage>();

        public static IWampConnection<TMessage> Instance
        {
            get
            {
                return mMockConnection.SideAToSideB;
            }
        }
    }
}