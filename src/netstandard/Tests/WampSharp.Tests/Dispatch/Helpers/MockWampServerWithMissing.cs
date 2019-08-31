using WampSharp.Core.Contracts;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Dispatch.Helpers
{
    public class MockWampServerWithMissingClient : MockWampServer, IWampMissingMethodContract<MockRaw, IWampClient>
    {
        public void Missing(IWampClient client, WampMessage<MockRaw> rawMessage)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MockWampServerWithMissing : MockWampServer, IWampMissingMethodContract<MockRaw>
    {
        public void Missing(WampMessage<MockRaw> rawMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}