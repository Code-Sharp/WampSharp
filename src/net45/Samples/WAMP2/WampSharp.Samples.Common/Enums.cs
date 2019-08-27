namespace WampSharp.Samples
{
    public enum Serialization
    {
        Json,
        Msgpack,
        Cbor
    }

    public enum Transport
    {
        WebSocket,
        Ws = WebSocket,
        RawSocket
    }
}
