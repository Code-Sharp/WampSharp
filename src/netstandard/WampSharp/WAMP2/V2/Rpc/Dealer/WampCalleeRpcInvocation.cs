using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampCalleeRpcInvocation : IWampCancellableInvocation
    {
        public IWampCallee Callee { get; }
        public long RequestId { get; }

        public WampCalleeRpcInvocation(IWampCallee callee, long requestId)
        {
            Callee = callee;
            RequestId = requestId;
        }

        public void Cancel(InterruptDetails details)
        {
            Callee.Interrupt(RequestId, details);
        }
    }
}