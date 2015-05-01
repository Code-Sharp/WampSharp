using Newtonsoft.Json;
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
        public DefaultWampCraHost(string location, IWampCraServerBuilder<JToken> serverBuilder) : 
            this(location, serverBuilder, new JsonSerializer())
        {
        }

        public DefaultWampCraHost(string location, IWampCraServerBuilder<JToken> serverBuilder, JsonSerializer serializer) : 
            base(location, serverBuilder, new JTokenMessageParser(serializer), new JsonFormatter(serializer))
        {
        }

        public DefaultWampCraHost(string location, WampCraAuthenticaticatorBuilder<JToken> craAuthenticaticatorBuilder) : 
            this(location, craAuthenticaticatorBuilder, new JsonSerializer())
        {
        }

        public DefaultWampCraHost(string location, WampCraAuthenticaticatorBuilder<JToken> craAuthenticaticatorBuilder, JsonSerializer serializer) :
            base(location, new JTokenMessageParser(serializer), new JsonFormatter(serializer), craAuthenticaticatorBuilder)
        {
        }
    }

    public class DefaultWampCraHost<TMessage> : WampCraHost<TMessage>
    {
        public DefaultWampCraHost(string location,
                                  IWampCraServerBuilder<TMessage> serverBuilder,
                                  IWampTextMessageParser<TMessage> parser,
                                  IWampFormatter<TMessage> formatter) :
                                      base(serverBuilder,
                                           new FleckWampConnectionListener<TMessage>(location, new Wamp1Binding<TMessage>(parser, formatter)),
                                           formatter)
        {
        }

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