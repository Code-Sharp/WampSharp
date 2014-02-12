namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationCallback<TMessage>
        where TMessage : class
    {
        void Result(TMessage details);

        void Result(TMessage details, TMessage[] arguments);

        void Result(TMessage details, TMessage[] arguments, TMessage argumentsKeywords);

        void Error(TMessage details, string error);

        void Error(TMessage details, string error, TMessage[] arguments);

        void Error(TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}