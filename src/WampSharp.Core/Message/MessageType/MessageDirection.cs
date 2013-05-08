namespace WampSharp.Core.Message
{
    public enum MessageDirection
    {
        AnyToAny,
        ServerToClient,
        ClientToServer,
        CallertoCallee = ClientToServer,
        CalleetoCaller = ServerToClient
    }
}