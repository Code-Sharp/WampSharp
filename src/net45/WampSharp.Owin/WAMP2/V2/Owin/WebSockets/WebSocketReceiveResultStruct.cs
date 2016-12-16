namespace WampSharp.Owin
{
    internal struct WebSocketReceiveResultStruct
    {
        public int MessageType;
        public bool EndOfMessage;
        public int Count;
    }
}