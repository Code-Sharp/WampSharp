using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2.Transports
{
    public class InMemoryTransport : IWampTransport
    {
        private readonly IDictionary<string, IDisposable> mBindings =
            new Dictionary<string, IDisposable>();

        private readonly IScheduler mRouterScheduler;

        public InMemoryTransport(IScheduler routerScheduler)
        {
            mRouterScheduler = routerScheduler;
        }

        public void Dispose()
        {
            foreach (IDisposable connectionListener in mBindings.Values)
            {
                connectionListener.Dispose();
            }
        }

        public void Open()
        {
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            return GetOrRegisterBinding(binding);
        }

        private IWampConnectionListener<TMessage> RegisterBinding<TMessage>(IWampBinding<TMessage> binding)
        {
            InMemoryConnectionListener<TMessage> result =
                new InMemoryConnectionListener<TMessage>(mRouterScheduler, binding);

            mBindings[binding.Name] = result;

            return result;
        }

        private InMemoryConnectionListener<TMessage> GetOrRegisterBinding<TMessage>(IWampBinding<TMessage> binding)
        {

            if (mBindings.TryGetValue(binding.Name, out IDisposable disposable))
            {
                return (InMemoryConnectionListener<TMessage>)disposable;
            }
            else
            {
                return (InMemoryConnectionListener<TMessage>)RegisterBinding(binding);
            }
        }

        public IControlledWampConnection<TMessage> CreateClientConnection<TMessage>
            (IWampBinding<TMessage> binding, IScheduler scheduler)
        {
            InMemoryConnectionListener<TMessage> casted =
                GetOrRegisterBinding(binding);

            return casted.CreateClientConnection(scheduler);
        }
    }
}