using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockWampRpcCallDetails<TMessage>
    {
        private readonly IWampRpcClient<TMessage> mClient;
        private readonly string mCallId;
        private readonly string mProcUri;
        private readonly object[] mArguments;

        public MockWampRpcCallDetails(IWampRpcClient<TMessage> client, string callId, string procUri, object[] arguments)
        {
            mClient = client;
            mCallId = callId;
            mProcUri = procUri;
            mArguments = arguments;
        }

        public IWampRpcClient<TMessage> Client
        {
            get
            {
                return mClient;
            }
        }

        public string CallId
        {
            get
            {
                return mCallId;
            }
        }

        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }

        public object[] Arguments
        {
            get
            {
                return mArguments;
            }
        }
    }
}