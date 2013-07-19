using Castle.DynamicProxy;
using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    /// <summary>
    /// An interceptor that serializes requests and sends them to a
    /// <see cref="IWampOutgoingMessageHandler{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampOutgoingInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingMessageHandler<TMessage> mOutgoingHandler;
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;

        /// <summary>
        /// Initializes a new instance of <see cref="WampOutgoingInterceptor{TMessage}"/>.
        /// </summary>
        /// <param name="outgoingSerializer">The <see cref="IWampOutgoingRequestSerializer{TMessage}"/> to
        /// serialize method calls with.</param>
        /// <param name="outgoingHandler">The <see cref="IWampOutgoingMessageHandler{TMessage}"/>
        /// that will deal with the serialized method calls.</param>
        public WampOutgoingInterceptor(IWampOutgoingRequestSerializer<TMessage> outgoingSerializer,
                                       IWampOutgoingMessageHandler<TMessage> outgoingHandler)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandler = outgoingHandler;
        }

        public void Intercept(IInvocation invocation)
        {
            WampMessage<TMessage> serialized =
                mOutgoingSerializer.SerializeRequest
                    (invocation.Method, invocation.Arguments);

            mOutgoingHandler.Handle(serialized);
        }
    }
}
