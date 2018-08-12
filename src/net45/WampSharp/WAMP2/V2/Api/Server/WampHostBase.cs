using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// A "base class" for all <see cref="IWampHost"/> implementations.
    /// </summary>
    internal class WampHostBase : IWampHost
    {
        private readonly IDictionary<IWampBinding, IWampBindingHost> mBindingToHost =
            new Dictionary<IWampBinding, IWampBindingHost>();
        private readonly ICollection<WampTransportDefinition> mTransportDefinitions =
            new List<WampTransportDefinition>();

        private readonly IWampUriValidator mUriValidator;
        private readonly IWampSessionMapper mSessionIdMap;

        /// <summary>
        /// Initializes a new instance of <see cref="WampHostBase"/>.
        /// </summary>
        public WampHostBase() :
            this(new WampRealmContainer(), new LooseUriValidator(), new WampSessionMapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampHostBase"/> given the
        /// <see cref="IWampRealmContainer"/> associated with this host.
        /// </summary>
        /// <param name="realmContainer"></param>
        /// <param name="uriValidator"></param>
        /// <param name="sessionIdMap"></param>
        public WampHostBase(IWampRealmContainer realmContainer, IWampUriValidator uriValidator, IWampSessionMapper sessionIdMap)
        {
            mUriValidator = uriValidator;
            RealmContainer = new HostedRealmContainer(realmContainer);
            mSessionIdMap = sessionIdMap;
        }

        public IWampHostedRealmContainer RealmContainer { get; }

        public void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            bindings = bindings ?? new IWampBinding[] {};
            
            IWampBinding[] bindingArray = bindings.ToArray();

            if (bindingArray.Any())
            {
                mTransportDefinitions.Add(new WampTransportDefinition()
                {
                    Transport = transport,
                    Bindings = bindingArray
                });
            }
            else
            {
                throw new ArgumentException("Got no binding. Expected at least one binding.", nameof(bindings));
            }
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
                binding.CreateHost(RealmContainer, compositeListener, mUriValidator, mSessionIdMap);
            
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