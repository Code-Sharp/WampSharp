using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCancelableInvocation
    {
        void Cancel(InterruptOptions options);
    }
}