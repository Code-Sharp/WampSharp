using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.V1.Core.Proxy
{
    /// <summary>
    /// An interceptor that serializes requests and sends them to a
    /// <see cref="IWampOutgoingMessageHandler"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampOutgoingInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingMessageHandler mOutgoingHandler;
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;

        /// <summary>
        /// Initializes a new instance of <see cref="WampOutgoingInterceptor{TMessage}"/>.
        /// </summary>
        /// <param name="outgoingSerializer">The <see cref="IWampOutgoingRequestSerializer"/> to
        /// serialize method calls with.</param>
        /// <param name="outgoingHandler">The <see cref="IWampOutgoingMessageHandler"/>
        /// that will deal with the serialized method calls.</param>
        public WampOutgoingInterceptor(IWampOutgoingRequestSerializer outgoingSerializer,
                                       IWampOutgoingMessageHandler outgoingHandler)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandler = outgoingHandler;
        }

        /// <summary>
        /// <see cref="IInterceptor.Intercept"/>
        /// </summary>
        public void Intercept(IInvocation invocation)
        {
            WampMessage<object> serialized =
                mOutgoingSerializer.SerializeRequest
                    (invocation.Method, invocation.Arguments);

            mOutgoingHandler.Handle(serialized);
        }
    }
}