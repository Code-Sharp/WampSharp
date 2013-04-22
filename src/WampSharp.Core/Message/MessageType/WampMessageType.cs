namespace WampSharp.Core.Message
{
    public enum WampMessageType
    {
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.Auxiliary)]
        Welcome = 0,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.Auxiliary)]
        Prefix = 1,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.RemoteProcedureCall)]
        Call = 2,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.RemoteProcedureCall)]
        CallResult = 3,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.RemoteProcedureCall)]
        CallError = 4,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe)]
        Subscribe = 5,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe)]
        Unsubscribe = 6,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe)]
        Publish = 7,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.PublishSubscribe)]
        Event = 8
    }
}