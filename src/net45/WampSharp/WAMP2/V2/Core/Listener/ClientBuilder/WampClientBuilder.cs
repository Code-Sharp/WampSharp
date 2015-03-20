using System;
using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Proxy;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilder{TMessage,TClient}"/>
    /// that is specific to WAMPv2.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilder<TMessage> : IWampClientBuilder<TMessage, IWampClientProxy<TMessage>>
    {
        #region Members

        private readonly IWampClientContainer<TMessage, IWampClientProxy<TMessage>> mContainer;
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly IWampIdGenerator mSessionIdGenerator;
        private readonly IWampBinding<TMessage> mBinding;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="sessionIdGenerator">A given <see cref="IWampIdGenerator"/> used in order
        /// to generate session ids for clients.</param>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer{TRequest}"/>
        /// used to serialize message calls into <see cref="WampMessage{TMessage}"/>s</param>
        /// <param name="outgoingHandlerBuilder">An <see cref="IWampOutgoingMessageHandlerBuilder{TMessage}"/> used to build
        /// a <see cref="IWampOutgoingMessageHandler{TMessage}"/> per connection.</param>
        /// <param name="container">A <see cref="IWampClientContainer{TMessage,TClient}"/> that contains all clients.</param>
        public WampClientBuilder(IWampIdGenerator sessionIdGenerator, IWampOutgoingRequestSerializer<TMessage> outgoingSerializer, IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampClientContainer<TMessage, IWampClientProxy<TMessage>> container, IWampBinding<TMessage> binding)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mContainer = container;
            mBinding = binding;
            mSessionIdGenerator = sessionIdGenerator;
        }

        #endregion

        public IWampClientProxy<TMessage> Create(IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler<TMessage> outgoingHandler = 
                mOutgoingHandlerBuilder.Build(connection);

            WampOutgoingInterceptor<TMessage> wampOutgoingInterceptor =
                new WampOutgoingInterceptor<TMessage>
                    (mOutgoingSerializer,
                     outgoingHandler);

            WampRawOutgoingInterceptor<TMessage> wampRawOutgoingInterceptor =
                new WampRawOutgoingInterceptor<TMessage>(outgoingHandler);

            ProxyGenerationOptions proxyGenerationOptions =
                new ProxyGenerationOptions()
                    {
                        Selector =
                            new WampInterceptorSelector<TMessage>
                            (wampOutgoingInterceptor,
                             wampRawOutgoingInterceptor)
                    };

            WampConnectionMonitor<TMessage> monitor = 
                new WampConnectionMonitor<TMessage>(connection);
            
            proxyGenerationOptions.AddMixinInstance(monitor);

            long session = mSessionIdGenerator.Generate();
            
            proxyGenerationOptions.AddMixinInstance
                (new WampClientContainerDisposable<TMessage, IWampClientProxy<TMessage>>
                    (mContainer, connection));

            WampClientPropertyBag<TMessage> propertyBag = 
                new WampClientPropertyBag<TMessage>(session, mBinding);
            
            proxyGenerationOptions.AddMixinInstance(propertyBag);

            IWampClientProxy<TMessage> result =
                mGenerator.CreateInterfaceProxyWithoutTarget
                    (typeof(IWampProxy), new[] { typeof(IWampClientProxy<TMessage>) },
                     proxyGenerationOptions,
                     wampOutgoingInterceptor)
                as IWampClientProxy<TMessage>;

            monitor.Client = result;

            return result;
        }
    }
}