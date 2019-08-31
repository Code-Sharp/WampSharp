using WampSharp.V2.Binding;
using WampSharp.V2.Client;

namespace WampSharp.V2.Fluent
{
    public class ChannelState : ChannelFactorySyntax.ISyntaxState,
        ChannelFactorySyntax.IAuthenticationSyntax,
        ChannelFactorySyntax.IBuildableSyntax,
        ChannelFactorySyntax.IRealmSyntax,
        ChannelFactorySyntax.ISerializationSyntax,
        ChannelFactorySyntax.ITransportSyntax,
        ChannelFactorySyntax.IObserveOnSyntax
    {
        public ChannelState()
        {
            Authenticator = new DefaultWampClientAuthenticator();
        }

        public string Realm { get; set; }

        public IWampConnectionActivator ConnectionActivator { get; set; }

        public IWampBinding Binding { get; set; }

        public IWampClientAuthenticator Authenticator { get; set; }

        public IWampChannelFactory ChannelFactory { get; set; }

        ChannelState ChannelFactorySyntax.ISyntaxState.State => this;

        IWampChannel ChannelFactorySyntax.IBuildableSyntax.Build()
        {
            dynamic connection = ConnectionActivator.Activate((dynamic)Binding);

            return ChannelFactory.CreateChannel(Realm, connection, (dynamic)Binding, Authenticator);
        }
    }
}