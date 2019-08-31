namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a text format <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampTextBinding<TMessage> : IWampTransportBinding<TMessage, string>
    {
    }
}