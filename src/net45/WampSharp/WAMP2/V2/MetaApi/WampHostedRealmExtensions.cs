using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Management;
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

            CompositeDisposable disposable = 
                HostDisposableService(hostedRealm, service, CalleeRegistrationInterceptor.Default);

            BrokerFeatures brokerFeatures = hostedRealm.Roles.Broker.Features;
            DealerFeatures dealerFeatures = hostedRealm.Roles.Dealer.Features;

            brokerFeatures.SessionMetaApi = true;
            brokerFeatures.SubscriptionMetaApi = true;
            dealerFeatures.SessionMetaApi = true;
            dealerFeatures.RegistrationMetaApi = true;

            IDisposable featureDisposable = Disposable.Create(() =>
            {
                brokerFeatures.SessionMetaApi = null;
                brokerFeatures.SubscriptionMetaApi = null;
                dealerFeatures.SessionMetaApi = null;
                dealerFeatures.RegistrationMetaApi = null;
            });

            disposable.Add(featureDisposable);

            return disposable;
        }

        /// <summary>
        /// Hosts a WAMP testament service for the given realm.
        /// </summary>
        /// <param name="hostedRealm">The given realm.</param>
        /// <returns>A disposable: disposing it will unregister the hosted testaments service.</returns>
        public static IDisposable HostTestamentService(this IWampHostedRealm hostedRealm)
        {
            WampTestamentService service = new WampTestamentService(hostedRealm);

            RegisterOptions registerOptions = new RegisterOptions { DiscloseCaller = true };

            CompositeDisposable disposable = HostDisposableService(hostedRealm, service, new CalleeRegistrationInterceptor(registerOptions));

            DealerFeatures dealerFeatures = hostedRealm.Roles.Dealer.Features;

            dealerFeatures.TestamentMetaApi = true;

            IDisposable featureDisposable = Disposable.Create(() =>
            {
                dealerFeatures.TestamentMetaApi = null;
            });

            disposable.Add(featureDisposable);

            return disposable;
        }

        private static CompositeDisposable HostDisposableService(IWampHostedRealm hostedRealm, IDisposable service, ICalleeRegistrationInterceptor registrationInterceptor)
        {
            Task<IAsyncDisposable> registrationDisposable =
                hostedRealm.Services.RegisterCallee(service, registrationInterceptor);

            IAsyncDisposable asyncDisposable = registrationDisposable.Result;

            IDisposable unregisterDisposable =
                Disposable.Create(() => asyncDisposable.DisposeAsync().Wait());

            CompositeDisposable result =
                new CompositeDisposable(unregisterDisposable, service);

            return result;
        }

        /// <summary>
        /// Hosts a WAMP session management service for the given realm.
        /// </summary>
        /// <param name="hostedRealm">The given realm.</param>
        /// <returns>A disposable: disposing it will unregister the hosted session management service.</returns>
        public static IDisposable HostSessionManagementService(this IWampHostedRealm hostedRealm, IWampUriValidator uriValidator = null)
        {
            WampSessionManagmentService service = new WampSessionManagmentService(hostedRealm, uriValidator);

            RegisterOptions registerOptions = new RegisterOptions { DiscloseCaller = true };

            CompositeDisposable disposable = HostDisposableService(hostedRealm, service, new CalleeRegistrationInterceptor(registerOptions));

            return disposable;
        }
    }
}