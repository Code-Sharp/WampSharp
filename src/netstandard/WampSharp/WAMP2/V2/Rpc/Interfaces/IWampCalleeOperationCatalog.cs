using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal interface IWampCalleeOperationCatalog
    {
        long Register(IRegisterRequest request, RegisterOptions options, string procedure);

        void Unregister(IWampCallee callee, long registrationId);
    }
}