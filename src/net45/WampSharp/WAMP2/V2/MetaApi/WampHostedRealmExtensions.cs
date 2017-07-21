using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.V2.Realm;
using WampSharp.V2.Testament;

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

            return HostDisposableService(hostedRealm, service);
        }

        /// <summary>
        /// Hosts a WAMP testaments service for the given realm.
        /// </summary>
        /// <param name="hostedRealm">The given realm.</param>
        /// <returns>A disposable: disposing it will unregister the hosted testaments service.</returns>
        public static IDisposable HostTestamentsService(this IWampHostedRealm hostedRealm)
        {
            WampTestamentService service = new WampTestamentService(hostedRealm);

            return HostDisposableService(hostedRealm, service);
        }

        private static IDisposable HostDisposableService(IWampHostedRealm hostedRealm, IDisposable service)
        {
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