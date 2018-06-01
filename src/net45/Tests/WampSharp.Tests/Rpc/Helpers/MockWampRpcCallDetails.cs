using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockWampRpcCallDetails<TMessage>
    {
        private readonly string mProcUri;

        public MockWampRpcCallDetails(IWampRpcClient<TMessage> client, string callId, string procUri, object[] arguments)
        {
            Client = client;
            CallId = callId;
            mProcUri = procUri;
            Arguments = arguments;
        }

        public IWampRpcClient<TMessage> Client { get; }

        public string CallId { get; }

        public string ProcUri => mProcUri;

        public object[] Arguments { get; }
    }
}