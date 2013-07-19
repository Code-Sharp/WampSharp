namespace WampSharp.Core.Message
{
    public interface IWampMessageFormatter<TMessage>
    {
        WampMessage<TMessage> Parse(TMessage message);
        TMessage Format(WampMessage<TMessage> message);
    }
}