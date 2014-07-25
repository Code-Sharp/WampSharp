using System;
using System.Collections.Generic;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public interface IWampHost : IDisposable
    {
        IWampHostedRealmContainer RealmContainer { get; }

        void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> binding);

        void Open();
    }
}