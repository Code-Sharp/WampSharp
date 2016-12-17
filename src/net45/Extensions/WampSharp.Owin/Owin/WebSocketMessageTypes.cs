namespace WampSharp.Owin
{
    /// <summary>Indicates the message type.</summary>
    public static class WebSocketMessageTypes
    {
        public const int Text = 0x1;
        public const int Binary = 0x2;
        public const int Close = 0x8;
    }
}