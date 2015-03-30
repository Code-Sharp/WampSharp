using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V1
{
    public class Wamp1Binding<TMessage> : JsonBinding<TMessage>
    {
        public Wamp1Binding(IWampTextMessageParser<TMessage> parser, IWampFormatter<TMessage> formatter) : 
            base(formatter, parser, "wamp")
        {
        }
    }

    public class Wamp1Binding : Wamp1Binding<JToken>
    {
        public Wamp1Binding() : this(new JsonSerializer())
        {
        }

        public Wamp1Binding(JsonSerializer serializer) : 
            base(new JTokenMessageParser(serializer), new JsonFormatter(serializer))
        {
        }
    }
}