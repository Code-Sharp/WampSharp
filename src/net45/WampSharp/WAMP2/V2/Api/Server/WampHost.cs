using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampHost"/>.
    /// </summary>
    public class WampHost : IWampHost
    {
        private readonly InMemoryWampHost mInternalHost;
        private readonly WampHostBase mExternalHost;
        private readonly ServiceHostedRealmContainer mRealmContainer;

        /// <summary>
        /// Initializes a new instance of <see cref="WampHost"/> given the
        /// <see cref="IWampRealmContainer"/> associated with this host.
        /// </summary>
        /// <param name="realmContainer"></param>
        /// <param name="uriValidator"></param>
        public WampHost(IWampRealmContainer realmContainer = null, IWampUriValidator uriValidator = null)
        {
            realmContainer = realmContainer ?? new WampRealmContainer();

            UriValidator = uriValidator ?? new LooseUriValidator();

            WampSessionMapper sessionIdMap = new WampSessionMapper();

            mInternalHost = new InMemoryWampHost(realmContainer, UriValidator, sessionIdMap);
            mInternalHost.Open();

            mExternalHost = new WampHostBase(realmContainer, UriValidator, sessionIdMap);

            mRealmContainer =
                new ServiceHostedRealmContainer(mExternalHost.RealmContainer,
                    mInternalHost);
        }

        public virtual void Dispose()
        {
            mInternalHost.Dispose();
            mExternalHost.Dispose();
        }

        public virtual IWampHostedRealmContainer RealmContainer => mRealmContainer;

        protected IWampUriValidator UriValidator { get; }

        public virtual void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            IEnumerable<IWampBinding> bindingArray = bindings.ToArray();
            
            mExternalHost.RegisterTransport(transport, bindingArray);

            foreach (IWampBinding currentBinding in bindingArray)
            {
                AddFormatter((dynamic) currentBinding);
            }
        }

        private void AddFormatter<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampFormatter<TMessage> formatter = binding.Formatter;

            mInternalHost.AddFormatter(formatter);
        }

        public virtual void Open()
        {
            mExternalHost.Open();
        }
    }
}