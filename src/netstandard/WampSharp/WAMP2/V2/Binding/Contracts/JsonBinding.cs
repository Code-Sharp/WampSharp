using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding.Contracts
{
    /// <summary>
    /// A base class that represents WAMP2 wamp.2.json binding.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class JsonBinding<TMessage> : WampTransportBinding<TMessage, string>,
        IWampTextBinding<TMessage>
    {
        protected JsonBinding(IWampFormatter<TMessage> formatter, IWampTextMessageParser<TMessage> parser, string protocolName = WampSubProtocols.JsonSubProtocol)
            : base(formatter, parser, protocolName)
        {
        }
    }
}