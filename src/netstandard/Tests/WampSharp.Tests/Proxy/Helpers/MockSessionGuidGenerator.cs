using WampSharp.V1.Core.Listener;

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