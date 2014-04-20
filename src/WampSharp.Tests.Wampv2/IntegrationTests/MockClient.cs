using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class MockClient<TClient>
    {
        private readonly TClient mClient;
        private readonly IMessageRecorder<MockRaw> mRecorder;

        public MockClient(TClient client, IMessageRecorder<MockRaw> recorder)
        {
            mClient = client;
            mRecorder = recorder;
        }

        public TClient Client
        {
            get
            {
                return mClient;
            }
        }

        public IMessageRecorder<MockRaw> Recorder
        {
            get
            {
                return mRecorder;
            }
        }
    }
}