using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    public interface IWampRpcOperationInvokerProxy : IWampRpcOperationInvokerProxy<object>
    {
         
    }

    public interface IWampRpcOperationInvokerProxy<TMessage>
    {
        void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure, TMessage[] arguments);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);    
    }
}