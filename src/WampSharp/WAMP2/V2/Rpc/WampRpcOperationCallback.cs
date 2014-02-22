using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCallback : IWampRpcOperationCallback
    {
        private readonly IWampCaller mCaller;
        private readonly long mRequestId;

        public WampRpcOperationCallback(IWampCaller caller, long requestId)
        {
            mCaller = caller;
            mRequestId = requestId;
        }

        public void Result(object details)
        {
            mCaller.Result(mRequestId, details);
        }

        public void Result(object details, object[] arguments)
        {
            mCaller.Result(mRequestId, details, arguments);
        }

        public void Result(object details, object[] arguments, object argumentsKeywords)
        {
            mCaller.Result(mRequestId, details, arguments, argumentsKeywords);
        }

        public void Error(object details, string error)
        {
            mCaller.CallError(mRequestId, details, error);
        }

        public void Error(object details, string error, object[] arguments)
        {
            mCaller.CallError(mRequestId, details, error, arguments);
        }

        public void Error(object details, string error, object[] arguments, object argumentsKeywords)
        {
            mCaller.CallError(mRequestId, details, error, arguments, argumentsKeywords);
        }
    }
}