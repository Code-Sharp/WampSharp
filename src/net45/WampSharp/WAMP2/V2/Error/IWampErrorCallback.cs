namespace WampSharp.V2.Error
{
    internal interface IWampErrorCallback
    {
        void Error(object details, string errorUri);
        void Error(object details, string errorUri, object[] arguments);
        void Error(object details, string errorUri, object[] arguments, object argumentsKeywords);
    }
}