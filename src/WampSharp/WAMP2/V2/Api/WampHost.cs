using System;
using System.Collections.Generic;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public class WampHost : IDisposable
    {
        private readonly IWampConnectionListenerProvider mListenerProvider; 
        private readonly IWampRealmContainer mRealmContainer;

        private readonly IDictionary<string, IDisposable> mBindingNameToHost =
            new Dictionary<string, IDisposable>();

        public WampHost(IWampConnectionListenerProvider connectionListener, IEnumerable<IWampBinding> bindings) :
            this(new WampRealmContainer(), connectionListener, bindings)
        {
        }

        public WampHost(IWampRealmContainer realmContainer, IWampConnectionListenerProvider listenerProvider, IEnumerable<IWampBinding> bindings)
        {
            mRealmContainer = realmContainer;
            mListenerProvider = listenerProvider;

            foreach (IWampBinding wampBinding in bindings)
            {
                Host((dynamic) wampBinding);
            }
        }

        public IWampRealmContainer RealmContainer
        {
            get
            {
                return mRealmContainer;
            }
        }

        protected void Host<TMessage>(IWampTextBinding<TMessage> binding)
        {
            IWampConnectionListener<TMessage> listener =
                mListenerProvider.GetTextListener(binding);

            Host(binding, listener);
        }

        protected void Host<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            IWampConnectionListener<TMessage> listener =
                mListenerProvider.GetBinaryListener(binding);

            Host(binding, listener);
        }

        private void Host<TMessage>(IWampBinding<TMessage> binding, IWampConnectionListener<TMessage> listener)
        {
            IWampBindingHost host =
                binding.CreateHost(RealmContainer, listener);

            host.Open();

            mBindingNameToHost[binding.Name] = host;
        }

        public void Open()
        {
            mListenerProvider.Open();
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in mBindingNameToHost.Values)
            {
                disposable.Dispose();
            }
        }
    }
}