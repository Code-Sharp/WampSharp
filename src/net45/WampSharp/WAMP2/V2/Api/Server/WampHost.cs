#if !PCL
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
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
        /// Initializes a new instance of <see cref="WampHost"/>.
        /// </summary>
        public WampHost() : this(new WampRealmContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampHost"/> given the
        /// <see cref="IWampRealmContainer"/> associated with this host.
        /// </summary>
        /// <param name="realmContainer"></param>
        public WampHost(IWampRealmContainer realmContainer)
        {
            mInternalHost = new InMemoryWampHost(realmContainer);
            mInternalHost.Open();

            mExternalHost = new WampHostBase(realmContainer);

            mRealmContainer =
                new ServiceHostedRealmContainer(mExternalHost.RealmContainer,
                    mInternalHost);
        }

        public void Dispose()
        {
            mInternalHost.Dispose();
            mExternalHost.Dispose();
        }

        public IWampHostedRealmContainer RealmContainer
        {
            get
            {
                return mRealmContainer;
            }
        }

        public void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            mExternalHost.RegisterTransport(transport, bindings);

            foreach (IWampBinding currentBinding in bindings)
            {
                AddFormatter((dynamic) currentBinding);
            }
        }

        private void AddFormatter<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampFormatter<TMessage> formatter = binding.Formatter;

            mInternalHost.AddFormatter(formatter);
        }

        public void Open()
        {
            mExternalHost.Open();
        }
    }
}
#endif