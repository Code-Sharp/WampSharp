using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampCalleeOperationCatalog<TMessage>
    {
        long Register(IWampCallee callee, TMessage options, string procedure);

        void Unregister(IWampCallee callee, long registrationId);
    }
}