using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCallback<TMessage> : IWampRpcOperationCallback<TMessage>
        where TMessage : class
    {
        private readonly IWampCaller mCaller;
        private readonly long mRequestId;

        public WampRpcOperationCallback(IWampCaller caller, long requestId)
        {
            mCaller = caller;
            mRequestId = requestId;
        }

        public void Result(TMessage details)
        {
            mCaller.Result(mRequestId, details);
        }

        public void Result(TMessage details, TMessage[] arguments)
        {
            mCaller.Result(mRequestId, details, arguments);
        }

        public void Result(TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller.Result(mRequestId, details, arguments, argumentsKeywords);
        }

        public void Error(TMessage details, string error)
        {
            mCaller.CallError(mRequestId, details, error);
        }

        public void Error(TMessage details, string error, TMessage[] arguments)
        {
            mCaller.CallError(mRequestId, details, error, arguments);
        }

        public void Error(TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller.CallError(mRequestId, details, error, arguments, argumentsKeywords);
        }
    }
}