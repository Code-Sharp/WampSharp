namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationInvoker<TMessage> where TMessage : class
    {
        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure);

        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure, TMessage[] arguments);

        void Invoke(IWampRpcOperationCallback<TMessage> caller, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);
    }
}