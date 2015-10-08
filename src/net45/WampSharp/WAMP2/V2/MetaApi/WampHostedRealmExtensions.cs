using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.V2.Realm;

namespace WampSharp.V2.MetaApi
{
    public static class WampHostedRealmExtensions
    {
        /// <summary>
        /// Hosts a WAMP meta-api service describing the given realm.
        /// </summary>
        /// <param name="hostedRealm">The given realm.</param>
        /// <returns>A disposable: disposing it will unregister the hosted meta-api service.</returns>
        public static IDisposable HostMetaApiService(this IWampHostedRealm hostedRealm)
        {
            WampRealmDescriptorService service = new WampRealmDescriptorService(hostedRealm);

            Task<IAsyncDisposable> registrationDisposable = 
                hostedRealm.Services.RegisterCallee(service);

            IAsyncDisposable asyncDisposable = registrationDisposable.Result;

            IDisposable unregisterDisposable =
                Disposable.Create(() => asyncDisposable.DisposeAsync().Wait());

            CompositeDisposable result = 
                new CompositeDisposable(unregisterDisposable, service);

            return result;
        }
    }
}