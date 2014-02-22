using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2
{
    public class DefaultWampHost : DefaultWampHost<JToken>
    {
        public DefaultWampHost(string location) :
            base(location, new JTokenMessageParser(), new JTokenBinding())
        {
        }
    }

    public class DefaultWampHost<TMessage> : WampHost<TMessage>
        where TMessage : class 
    {
        public DefaultWampHost(string location,
                               IWampMessageParser<TMessage> parser,
                               IWampBinding<TMessage> binding) :
            this(new FleckWampConnectionListener<TMessage>(binding.Name, location, parser), binding)
        {
        }

        public DefaultWampHost(IWampConnectionListener<TMessage> connectionListener, IWampBinding<TMessage> binding)
            : base(connectionListener, binding)
        {
        }
    }
}