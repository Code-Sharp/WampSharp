using WampSharp.Core.Listener;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class MockSessionGuidGenerator : IWampSessionIdGenerator
    {
        public string Generate()
        {
            return "randomSessionId1";
        }
    }
}