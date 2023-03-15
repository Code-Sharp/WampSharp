using System.Collections.Generic;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2.Authentication
{
    public interface IWampMultiAuthenticationHost : IWampHost
    {
        void RegisterTransport(IWampTransport transport,
                               IDictionary<IWampBinding, IWampSessionAuthenticatorFactory> bindingToAuthenticatorFactory);
    }
}