namespace WampSharp.Owin
{
    /// <summary>Indicates the message type.</summary>
    public static class WebSocketMessageType
    {
        public static readonly int Text = 0x1;
        public static readonly int Binary = 0x2;
        public static readonly int Close = 0x8;
    }
}