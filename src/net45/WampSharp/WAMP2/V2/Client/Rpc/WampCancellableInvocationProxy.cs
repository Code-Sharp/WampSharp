using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampCancellableInvocationProxy : IWampCancellableInvocationProxy
    {
        public IWampServerProxy Proxy { get; }
        public long RequestId { get; }

        public WampCancellableInvocationProxy(IWampServerProxy proxy, long requestId)
        {
            Proxy = proxy;
            RequestId = requestId;
        }

        public void Cancel(CancelOptions options)
        {
            Proxy.Cancel(RequestId, options);
        }
    }
}