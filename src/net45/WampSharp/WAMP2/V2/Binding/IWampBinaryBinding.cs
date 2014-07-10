namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a binary format <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampBinaryBinding<TMessage> : IWampTransportBinding<TMessage, byte[]>
    {
    }
}