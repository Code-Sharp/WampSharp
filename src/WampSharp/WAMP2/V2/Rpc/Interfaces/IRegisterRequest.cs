using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IRegisterRequest
    {
        IWampCallee Callee { get; }

        void Registered(long registrationId);
    }
}