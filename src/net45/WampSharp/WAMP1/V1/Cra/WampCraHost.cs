using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.V1.Cra
{
    public class WampCraHost<TMessage> : WampHost<TMessage>
    {
        public WampCraHost(IWampConnectionListener<TMessage> connectionListener,
            IWampFormatter<TMessage> formatter,
            WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder) :
                this(new WampCraServerBuilder<TMessage>(craAuthenticaticatorBuilder), connectionListener, formatter)
        {
        }

        public WampCraHost(IWampCraServerBuilder<TMessage> serverBuilder,
            IWampConnectionListener<TMessage> connectionListener,
            IWampFormatter<TMessage> formatter)
            : base(serverBuilder, connectionListener, formatter)
        {
            serverBuilder.InitializeAuthenticationBuilder(formatter, base.MetadataCatalog, base.TopicContainerExtended);
        }
    }
}