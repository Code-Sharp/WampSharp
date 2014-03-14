using WampSharp.Binding;
using WampSharp.Core.Serialization;

namespace WampSharp.V1
{
    internal class Wamp1Binding<TMessage> : JsonBinding<TMessage>
    {
        public Wamp1Binding(IWampTextMessageParser<TMessage> parser, IWampFormatter<TMessage> formatter) : 
            base(formatter, parser, "wamp")
        {
        }
    }
}