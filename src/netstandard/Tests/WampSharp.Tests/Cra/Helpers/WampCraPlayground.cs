using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V1;
using WampSharp.V1.Cra;

namespace WampSharp.Tests.Cra.Helpers
{
    public class WampCraPlayground<TMessage> : WampPlayground<TMessage>
    {
        public WampCraPlayground(IWampFormatter<TMessage> formatter, 
                                    WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder) : 
                                        this(formatter, new MockConnectionListener<TMessage>(formatter), craAuthenticaticatorBuilder)
        {
        }

        protected WampCraPlayground(IWampFormatter<TMessage> formatter, 
                                    MockConnectionListener<TMessage> listener, 
                                    WampCraAuthenticaticatorBuilder<TMessage> craAuthenticaticatorBuilder) : 
                                        this(formatter, listener, 
                                             new WampCraHost<TMessage>(listener, formatter, craAuthenticaticatorBuilder))
        {
        }

        protected WampCraPlayground(IWampFormatter<TMessage> formatter, MockConnectionListener<TMessage> listener, IWampHost host) : base(formatter, listener, host)
        {
        }
    }
}