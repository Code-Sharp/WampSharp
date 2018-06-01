using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class MockClient<TClient>
    {
        private readonly IMessageRecorder<MockRaw> mRecorder;

        public MockClient(TClient client, IMessageRecorder<MockRaw> recorder)
        {
            Client = client;
            mRecorder = recorder;
        }

        public TClient Client { get; }

        public IMessageRecorder<MockRaw> Recorder => mRecorder;
    }
}