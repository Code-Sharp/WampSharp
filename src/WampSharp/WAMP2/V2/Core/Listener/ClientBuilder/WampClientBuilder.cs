using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Core.Contracts;
using SessionIdPropertyInterceptor = WampSharp.V2.Core.Proxy.SessionIdPropertyInterceptor;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilder{TMessage,TClient}"/>
    /// that is a bit specific to WAMPv1 (because of curies).
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilder<TMessage> : IWampClientBuilder<TMessage, IWampClient>
    {
        #region Members

        private readonly IWampClientContainer<TMessage, IWampClient> mContainer;
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly IWampIdGenerator mSessionIdGenerator;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="sessionIdGenerator">A given <see cref="WampSharp.Core.Listener.IWampSessionIdGenerator"/> used in order
        /// to generate session ids for clients.</param>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer{TRequest}"/>
        /// used to serialize message calls into <see cref="WampMessage{TMessage}"/>s</param>
        /// <param name="outgoingHandlerBuilder">An <see cref="IWampOutgoingMessageHandlerBuilder{TMessage}"/> used to build
        /// a <see cref="IWampOutgoingMessageHandler{TMessage}"/> per connection.</param>
        /// <param name="container">A <see cref="IWampClientContainer{TMessage,TClient}"/> that contains all clients.</param>
        public WampClientBuilder(IWampIdGenerator sessionIdGenerator, IWampOutgoingRequestSerializer<TMessage> outgoingSerializer, IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampClientContainer<TMessage, IWampClient> container)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mContainer = container;
            mSessionIdGenerator = sessionIdGenerator;
        }

        #endregion

        public IWampClient Create(IWampConnection<TMessage> connection)
        {
            WampOutgoingInterceptor<TMessage> wampOutgoingInterceptor =
                new WampOutgoingInterceptor<TMessage>
                    (mOutgoingSerializer,
                     mOutgoingHandlerBuilder.Build(connection));

            ProxyGenerationOptions proxyGenerationOptions =
                new ProxyGenerationOptions()
                    {
                        Selector =
                            new Proxy.WampInterceptorSelector<TMessage>
                            (wampOutgoingInterceptor)
                    };

            proxyGenerationOptions.AddMixinInstance
                (new WampClientContainerDisposable<TMessage, IWampClient>
                    (mContainer, connection));

            IWampClient result =
                mGenerator.CreateInterfaceProxyWithoutTarget<IWampClient>
                    (proxyGenerationOptions, wampOutgoingInterceptor,
                    new SessionIdPropertyInterceptor(mSessionIdGenerator.Generate()));

            return result;
        }
    }
}