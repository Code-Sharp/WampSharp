using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    public class WampMockClientBuilder<TMessage>
    {
        #region Members

        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;

        public WampMockClientBuilder(IWampFormatter<TMessage> formatter)
        {
            mOutgoingSerializer =
                new WampOutgoingRequestSerializer<TMessage>(formatter);
        }

        #endregion

        public IWampClient<TMessage> Create(long sessionId,
                                            IMessagePlayer<TMessage> player,
                                            IMessageRecorder<TMessage> recorder)
        {
            ProxyGenerationOptions options =
                new ProxyGenerationOptions();

            options.Selector = new MockClientInterceptorSelector();

            IWampClient<TMessage> result =
                mGenerator.CreateInterfaceProxyWithoutTarget
                    (typeof (IWampClient),
                     new[]
                         {
                             typeof (IWampClient<TMessage>),
                             typeof (IWampConnectionMonitor)
                         },
                         options,
                     new RecordAndPlayInterceptor<TMessage>
                         (mOutgoingSerializer, player, recorder),
                     new SessionPropertyInterceptor(sessionId))
                as IWampClient<TMessage>;

            return result;
        }
    }
}