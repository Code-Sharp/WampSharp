using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V1
{
    public class DefaultWampHost : DefaultWampHost<JToken>
    {
        public DefaultWampHost(string location) :
            this(location, new JsonSerializer())
        {
        }

        public DefaultWampHost(string location, JsonSerializer serializer) :
            base(location, new JTokenMessageParser(serializer), new JsonFormatter(serializer))
        {
        }

        public DefaultWampHost(string location, IWampServerBuilder<JToken> serverBuilder) :
            this(location, serverBuilder, new JsonSerializer())
        {
        }

        public DefaultWampHost(string location, IWampServerBuilder<JToken> serverBuilder, JsonSerializer serializer) :
            base(location, serverBuilder, new JTokenMessageParser(serializer), new JsonFormatter(serializer))
        {
        }
    }

    public class DefaultWampHost<TMessage> : WampHost<TMessage>
    {
        public DefaultWampHost(string location,
                               IWampServerBuilder<TMessage> serverBuilder,
                               IWampTextMessageParser<TMessage> parser,
                               IWampFormatter<TMessage> formatter) :
                                   base(serverBuilder,
                                        new FleckWampConnectionListener<TMessage>(location, new Wamp1Binding<TMessage>(parser, formatter)),
                                        formatter)
        {
        }

        public DefaultWampHost(string location,
                               IWampTextMessageParser<TMessage> parser,
                               IWampFormatter<TMessage> formatter) :
                                   this(new FleckWampConnectionListener<TMessage>(location, new Wamp1Binding<TMessage>(parser, formatter)), formatter)
        {
        }

        public DefaultWampHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
            : base(connectionListener, formatter)
        {
        }

        public DefaultWampHost(IWampServerBuilder<TMessage> serverBuilder, IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter) : 
            base(serverBuilder, connectionListener, formatter)
        {
        }
    }
}