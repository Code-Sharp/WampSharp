namespace WampSharp.V2.RPC
{
    public interface IWampRpcOperation<TMessage> where TMessage : class
    {
        string Procedure { get; }

        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options);

        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, TMessage[] arguments);

        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}