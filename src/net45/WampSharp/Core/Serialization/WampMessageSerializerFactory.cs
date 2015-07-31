#if CASTLE
using Castle.DynamicProxy;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// An implementation of <see cref="IWampMessageSerializerFactory"/> using
    /// <see cref="ProxyGenerator"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampMessageSerializerFactory<TMessage> : IWampMessageSerializerFactory
    {
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly WampSerializationInterceptor<TMessage> mSerializationInterceptor;

        /// <summary>
        /// Creates a new instance of <see cref="WampMessageSerializerFactory{TMessage}"/>
        /// given a <see cref="IWampOutgoingRequestSerializer"/>, used to serialize
        /// message.
        /// </summary>
        /// <param name="serializer">The given <see cref="IWampOutgoingRequestSerializer"/>.</param>
        public WampMessageSerializerFactory(IWampOutgoingRequestSerializer serializer)
        {
            mSerializationInterceptor = new WampSerializationInterceptor<TMessage>(serializer);
        }

        public TProxy GetSerializer<TProxy>() where TProxy : class
        {
            TProxy result =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>
                (mSerializationInterceptor);

            return result;
        }
    }
}
#endif
