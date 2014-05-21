using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;

namespace WampSharp
{
    public class DefaultWampHost : DefaultWampHost<JToken>
    {
        public DefaultWampHost(string location) :
            base(location, new JTokenMessageParser(), new JsonFormatter())
        {
        }
    }

    public class DefaultWampHost<TMessage> : WampHost<TMessage>
    {
        public DefaultWampHost(string location,
                               IWampMessageParser<TMessage> parser,
                               IWampFormatter<TMessage> formatter) :
                                   this(new FleckWampConnectionListener<TMessage>(location, parser), formatter)
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