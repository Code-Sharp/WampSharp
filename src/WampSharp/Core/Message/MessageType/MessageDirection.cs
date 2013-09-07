namespace WampSharp.Core.Message
{
#pragma warning disable 1591
    public enum MessageDirection
    {
        AnyToAny,
        ServerToClient,
        ClientToServer,
        CallertoCallee = ClientToServer,
        CalleetoCaller = ServerToClient
    }
#pragma warning restore 1591
}