#if CASTLE
using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// Represents a <see cref="IInterceptor"/> used for <see cref="WampMessageSerializerFactory{TMessage}"/>
    /// mechanism.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampSerializationInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingRequestSerializer mSerializer;

        /// <summary>
        /// Initializes a new instance of <see cref="WampSerializationInterceptor{TMessage}"/>
        /// given the <see cref="IWampOutgoingRequestSerializer"/> used to serialize
        /// method calls to messages.
        /// </summary>
        /// <param name="serializer">The given <see cref="IWampOutgoingRequestSerializer"/>.</param>
        public WampSerializationInterceptor(IWampOutgoingRequestSerializer serializer)
        {
            mSerializer = serializer;
        }

        /// <summary>
        /// <see cref="IInterceptor.Intercept"/>
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            WampMessage<object> result =
                mSerializer.SerializeRequest(invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}
#endif
