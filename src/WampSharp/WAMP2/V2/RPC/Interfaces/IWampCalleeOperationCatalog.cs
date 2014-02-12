using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.RPC
{
    public interface IWampCalleeOperationCatalog<TMessage>
        where TMessage : class
    {
        long Register(IWampCallee callee, TMessage options, string procedure);

        void Unregister(IWampCallee callee);

        void Unregister(IWampCallee callee, long registrationId);
    }
}