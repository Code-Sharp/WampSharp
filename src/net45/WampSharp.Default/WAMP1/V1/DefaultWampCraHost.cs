using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;
using WampSharp.V1.Cra;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V1
{
    public class DefaultWampCraHost : DefaultWampCraHost<JToken>
    {
        public DefaultWampCraHost(string location, WampCraAuthenticaticatorBuilder<JToken> craAuthenticaticatorBuilder) :
            base(location, new JTokenMessageParser(), new JsonFormatter(), craAuthenticaticatorBuilder)
        {
        }
    }

    public class DefaultWampCraHost<TMessage> : WampCraHost<TMessage>
    {
        public DefaultWampCraHost(string location,
                               IWampTextMessageParser<TMessage> parser,
                               IWampFormatter<TMessage> formatter,
                               WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder) :
            this(new FleckWampConnectionListener<TMessage>(location, new Wamp1Binding<TMessage>(parser, formatter)), formatter, craAuthenticaticatorBuilder)
        {
        }

        public DefaultWampCraHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter, WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder)
            : base(connectionListener, formatter, craAuthenticaticatorBuilder)
        {
        }

        public DefaultWampCraHost(IWampCraServerBuilder<TMessage> serverBuilder, IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter) :
            base(serverBuilder, connectionListener, formatter)
        {
        }
    }
}
