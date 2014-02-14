namespace WampSharp.Core.Message
{
    internal enum MessageCategory
    {
        General,
        Auxiliary,
        RemoteProcedureCall,
        PublishSubscribe,
        Session = Auxiliary
    }
}