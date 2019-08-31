using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    public class WampAuthenticationPlayground : WampAuthenticationPlayground<JToken>
    {
        public WampAuthenticationPlayground(IWampSessionAuthenticatorFactory sessionAuthenticationFactory) : 
            base(new JTokenJsonBinding(), sessionAuthenticationFactory)
        {
        }
    }

    public class WampAuthenticationPlayground<TMessage> : WampPlayground<TMessage>
    {
        public WampAuthenticationPlayground(IWampBinding<TMessage> binding,
                                            IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : this(binding, new MockConnectionListener<TMessage>(binding.Formatter),
                   EqualityComparer<TMessage>.Default,
                   sessionAuthenticationFactory)
        {
        }

        protected WampAuthenticationPlayground(IWampBinding<TMessage> binding,
                                               MockConnectionListener<TMessage> listener,
                                               IEqualityComparer<TMessage> equalityComparer,
                                               IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : this(binding, listener, GetHost(binding, listener, sessionAuthenticationFactory), equalityComparer)
        {
        }

        private static IWampHost GetHost(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener, IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            WampAuthenticationHost result = new WampAuthenticationHost(sessionAuthenticationFactory);

            result.RegisterTransport(new MockTransport<TMessage>(listener), binding);

            return result;
        }

        protected WampAuthenticationPlayground(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener, IWampHost host, IEqualityComparer<TMessage> equalityComparer) :
            base(binding, listener, host, equalityComparer)
        {
        }
    }
}