namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperation<TMessage> where TMessage : class
    {
        string Procedure { get; }

        void Invoke(IWampRpcOperationCallback caller, TMessage options);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}