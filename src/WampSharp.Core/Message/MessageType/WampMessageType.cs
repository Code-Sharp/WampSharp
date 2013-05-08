namespace WampSharp.Core.Message
{
    public enum WampMessageType
    {
        #region Version 1

        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.Auxiliary, 1)]
        v1Welcome = 0,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.Auxiliary, 1)]
        v1Prefix = 1,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.RemoteProcedureCall, 1)]
        v1Call = 2,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.RemoteProcedureCall, 1)]
        v1CallResult = 3,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.RemoteProcedureCall, 1)]
        v1CallError = 4,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe, 1)]
        v1Subscribe = 5,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe, 1)]
        v1Unsubscribe = 6,
        [MessageTypeDetails(MessageDirection.ClientToServer, MessageCategory.PublishSubscribe, 1)]
        v1Publish = 7,
        [MessageTypeDetails(MessageDirection.ServerToClient, MessageCategory.PublishSubscribe, 1)]
        v1Event = 8,

        #endregion

        #region Version 2

        [MessageTypeDetails(MessageDirection.AnyToAny, MessageCategory.Auxiliary, 2)]
        v2Hello = 0,
        [MessageTypeDetails(MessageDirection.AnyToAny, MessageCategory.Auxiliary, 2)]
        v2Heartbeat = 1,
        [MessageTypeDetails(MessageDirection.AnyToAny, MessageCategory.Auxiliary, 2)]
        v2Goodbye = 2,

        [MessageTypeDetails(MessageDirection.CallertoCallee, MessageCategory.RemoteProcedureCall, 2)]
        v2Call = 16 + 0,
        [MessageTypeDetails(MessageDirection.CallertoCallee, MessageCategory.RemoteProcedureCall, 2)]
        v2CallCancel = 16 + 1,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.RemoteProcedureCall, 2)]
        v2CallResult = 32 + 0,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.RemoteProcedureCall, 2)]
        v2CallProgress = 32 + 1,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.RemoteProcedureCall, 2)]
        v2CallError = 32 + 2,

        [MessageTypeDetails(MessageDirection.CallertoCallee, MessageCategory.PublishSubscribe, 2)]
        v2Subscribe = 64 + 0,
        [MessageTypeDetails(MessageDirection.CallertoCallee, MessageCategory.PublishSubscribe, 2)]
        v2Unsubscribe = 64 + 1,
        [MessageTypeDetails(MessageDirection.CallertoCallee, MessageCategory.PublishSubscribe, 2)]
        v2Publish = 64 + 2,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.PublishSubscribe, 2)]
        v2Event = 128 + 0,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.PublishSubscribe, 2)]
        v2Metaevent = 128 + 1,
        [MessageTypeDetails(MessageDirection.CalleetoCaller, MessageCategory.PublishSubscribe, 2)]
        v2PublishAck = 128 + 2,
        
        #endregion
    }
}