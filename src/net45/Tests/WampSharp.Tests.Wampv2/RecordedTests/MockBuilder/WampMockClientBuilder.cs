using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class WampMockClientBuilder<TMessage>
    {
        private readonly WampBinding<TMessage> mBinding;

        #region Members

        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;

        public WampMockClientBuilder(WampBinding<TMessage> binding)
        {
            mBinding = binding;
            mOutgoingSerializer =
                new WampOutgoingRequestSerializer<TMessage>(binding.Formatter);
        }

        #endregion

        public IWampClientProxy<TMessage> Create(long sessionId,
                                            IMessagePlayer<TMessage> player,
                                            IMessageRecorder<TMessage> recorder)
        {
            ProxyGenerationOptions options =
                new ProxyGenerationOptions();

            options.Selector = new MockClientInterceptorSelector();

            IWampClientProxy<TMessage> result =
                mGenerator.CreateInterfaceProxyWithoutTarget
                    (typeof (IWampClientProxy),
                     new[]
                         {
                             typeof (IWampClientProxy<TMessage>),
                             typeof (IWampConnectionMonitor)
                         },
                     options,
                     new RecordAndPlayRawInterceptor<TMessage>(player, recorder, mBinding),
                     new RecordAndPlayInterceptor<TMessage>
                         (mOutgoingSerializer, player, recorder, mBinding),
                     new SessionPropertyInterceptor(sessionId))
                as IWampClientProxy<TMessage>;

            return result;
        }
    }
}