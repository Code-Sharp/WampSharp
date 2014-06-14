namespace WampSharp.V2
{
    public interface IWampSerializedEvent<TMessage>
    {
        long PublicationId { get; }
        TMessage Details { get; }
        TMessage[] Arguments { get; }
        TMessage ArgumentsKeywords { get; }
    }

    public interface IWampSerializedEvent : IWampSerializedEvent<ISerializedValue>
    {
    }
}