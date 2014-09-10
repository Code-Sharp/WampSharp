using System.Collections.Generic;
using System.Linq;
using WampSharp.Binding;
using WampSharp.Fleck;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public class DefaultWampHost : WampHost
    {
        public DefaultWampHost(string location) :
            this(location, new IWampBinding[] { new JTokenBinding(), new JTokenMsgpackObjectBinding() })
        {
        }

        public DefaultWampHost(string location, IEnumerable<IWampBinding> bindings)
            : this(location, new WampRealmContainer(), bindings)
        {
        }

        public DefaultWampHost(string location, IWampRealmContainer realmContainer, IEnumerable<IWampBinding> bindings)
            : base(realmContainer)
        {
            this.RegisterTransport(new FleckWebSocketTransport(location),
                                   bindings.ToArray());
        }
    }
}