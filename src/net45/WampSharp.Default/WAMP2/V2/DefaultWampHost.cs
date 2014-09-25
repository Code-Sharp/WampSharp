using System.Collections.Generic;
using System.Linq;
using WampSharp.Binding;
using WampSharp.Fleck;
using WampSharp.V2.Binding;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="WampHost"/> that uses
    /// <see cref="FleckWebSocketTransport"/> internally.
    /// </summary>
    public class DefaultWampHost : WampHost
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with Json and Msgpack support.
        /// </summary>
        /// <param name="location">The given location.</param>
        public DefaultWampHost(string location) :
            this(location, new IWampBinding[] { new JTokenJsonBinding(), new JTokenMsgpackBinding() })
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="bindings">The given bindings.</param>
        public DefaultWampHost(string location, IEnumerable<IWampBinding> bindings)
            : this(location, new WampRealmContainer(), bindings)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="realmContainer">The given <see cref="IWampRealmContainer"/>.</param>
        /// <param name="bindings">The given bindings.</param>
        public DefaultWampHost(string location, IWampRealmContainer realmContainer, IEnumerable<IWampBinding> bindings)
            : base(realmContainer)
        {
            this.RegisterTransport(new FleckWebSocketTransport(location),
                                   bindings.ToArray());
        }
    }
}