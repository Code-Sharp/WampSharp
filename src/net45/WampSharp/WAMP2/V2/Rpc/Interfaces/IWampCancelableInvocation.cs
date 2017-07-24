using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCancelableInvocation
    {
        long RegistrationId { get; set; }
        void Cancel(InterruptOptions options);
    }
}