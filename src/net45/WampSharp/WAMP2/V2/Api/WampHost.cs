using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public class WampHost : IWampHost
    {
        private readonly IDictionary<IWampBinding, IWampBindingHost> mBindingToHost =
            new Dictionary<IWampBinding, IWampBindingHost>();

        private readonly IWampRealmContainer mRealmContainer;

        private readonly ICollection<WampTransportDefinition> mTransportDefinitions =
            new List<WampTransportDefinition>();

        public WampHost() : this(new WampRealmContainer())
        {
        }

        public WampHost(IWampRealmContainer realmContainer)
        {
            mRealmContainer = realmContainer;
        }

        public IWampRealmContainer RealmContainer
        {
            get { return mRealmContainer; }
        }

        public void RegisterTransport(IWampTransport transport,
                                      params IWampBinding[] binding)
        {
            RegisterTransport(transport, binding);
        }

        public void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> binding)
        {
            mTransportDefinitions.Add(new WampTransportDefinition()
                {
                    Transport = transport,
                    Bindings = binding.ToArray()
                });
        }

        private void InitializeBindingHosts()
        {
            var transportByBinding =
                from definition in mTransportDefinitions
                from binding in definition.Bindings
                group definition.Transport by binding;

            foreach (IGrouping<IWampBinding, IWampTransport> bindingToTransports in transportByBinding)
            {
                IWampBinding binding = bindingToTransports.Key;

                IWampBindingHost host = 
                    GetBindingHost((dynamic)binding, bindingToTransports);

                mBindingToHost[binding] = host;
            }
        }

        private IWampBindingHost GetBindingHost<TMessage>
            (IWampBinding<TMessage> binding,
             IEnumerable<IWampTransport> transports)
        {
            List<IWampConnectionListener<TMessage>> listeners = 
                new List<IWampConnectionListener<TMessage>>();
            
            foreach (IWampTransport transport in transports)
            {
                IWampConnectionListener<TMessage> listener = transport.GetListener(binding);
                listeners.Add(listener);
            }

            IWampConnectionListener<TMessage> compositeListener =
                new CompositeListener<TMessage>(listeners);

            IWampBindingHost host = 
                binding.CreateHost(RealmContainer, compositeListener);
            
            return host;
        }

        public void Open()
        {
            InitializeBindingHosts();

            OpenBindingHosts();

            OpenTransports();
        }

        private void OpenBindingHosts()
        {
            foreach (IWampBindingHost bindingHost in mBindingToHost.Values)
            {
                bindingHost.Open();
            }
        }

        private void OpenTransports()
        {
            foreach (WampTransportDefinition transportDefinition in mTransportDefinitions)
            {
                IWampTransport transport = transportDefinition.Transport;
                transport.Open();
            }
        }

        public void Dispose()
        {
            foreach (WampTransportDefinition transportDefinition in mTransportDefinitions)
            {
                IWampTransport transport = transportDefinition.Transport;
                transport.Dispose();
            }

            foreach (IWampBindingHost bindingHost in mBindingToHost.Values)
            {
                bindingHost.Dispose();
            }
        }
    }
}