using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Cra;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;

namespace WampSharp
{
    public class DefaultWampCraHost : DefaultWampCraHost<JToken>
    {
        public DefaultWampCraHost(string location, IWampCraServerBuilder<JToken> serverBuilder) : 
            base(location, serverBuilder, new JTokenMessageParser(), new JsonFormatter())
        {
        }

        public DefaultWampCraHost(string location, WampCraAuthenticaticatorBuilder<JToken> craAuthenticaticatorBuilder) :
            base(location, new JTokenMessageParser(), new JsonFormatter(), craAuthenticaticatorBuilder)
        {
        }
    }

    public class DefaultWampCraHost<TMessage> : WampCraHost<TMessage>
    {
        public DefaultWampCraHost(string location,
                                  IWampCraServerBuilder<TMessage> serverBuilder,
                                  IWampMessageParser<TMessage> parser,
                                  IWampFormatter<TMessage> formatter) :
                                      base(serverBuilder,
                                           new FleckWampConnectionListener<TMessage>(location, parser), formatter)
        {
        }

        public DefaultWampCraHost(string location,
                               IWampMessageParser<TMessage> parser,
                               IWampFormatter<TMessage> formatter,
                               WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder) :
            this(new FleckWampConnectionListener<TMessage>(location, parser), formatter, craAuthenticaticatorBuilder)
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