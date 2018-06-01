using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.MetaApi;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Proxy
{
    internal class WampClientPropertyBag<TMessage> : IWampClientProperties, IWampClientProperties<TMessage>
    {
        private readonly WampTransportDetails mTransportDetails;

        public WampClientPropertyBag(IWampBinding<TMessage> binding, WampTransportDetails transportDetails)
        {
            Binding = binding;
            mTransportDetails = transportDetails;
        }

        public IWampSessionAuthenticator Authenticator { get; set; }

        public IWampAuthorizer Authorizer => Authenticator.Authorizer;

        public HelloDetails HelloDetails { get; set; }

        public WelcomeDetails WelcomeDetails { get; set; }

        public bool GoodbyeSent { get; set; }

        public long Session { get; set; }

        IWampBinding IWampClientProperties.Binding => this.Binding;

        public WampTransportDetails TransportDetails => mTransportDetails;

        public IWampBindedRealm<TMessage> Realm { get; set; }

        public IWampBinding<TMessage> Binding { get; }
    }
}