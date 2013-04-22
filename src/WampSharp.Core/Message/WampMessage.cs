namespace WampSharp.Core.Message
{
    public class WampMessage<TMessage>
    {
        public WampMessageType MessageType { get; set; }

        public TMessage[] Arguments { get; set; }
    }
}