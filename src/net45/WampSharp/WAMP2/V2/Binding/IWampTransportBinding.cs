using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a raw format <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRaw"></typeparam>
    public interface IWampTransportBinding<TMessage, TRaw> :
        IWampBinding<TMessage>,
        IWampMessageParser<TMessage, TRaw>
    {
        bool? ComputeBytes { get; set; }
    }
}