using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Contracts;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Realm;

namespace WampSharp.V1
{
    public class Wamp1Binding<TMessage> : JsonBinding<TMessage>
    {
        public Wamp1Binding(IWampTextMessageParser<TMessage> parser, IWampFormatter<TMessage> formatter) : 
            base(formatter, parser, "wamp")
        {
        }

        public override IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            return null;
        }
    }

    public class Wamp1Binding : Wamp1Binding<JToken>
    {
        public Wamp1Binding() : 
            base(new JTokenMessageParser(), new JsonFormatter())
        {
        }
    }
}