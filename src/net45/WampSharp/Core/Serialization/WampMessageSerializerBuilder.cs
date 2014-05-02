using Castle.DynamicProxy;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Serialization
{
    public class WampMessageSerializerBuilder<TMessage> : IWampMessageSerializerBuilder
    {
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly WampSerializationInterceptor<TMessage> mSerializationInterceptor;

        public WampMessageSerializerBuilder(IWampOutgoingRequestSerializer<TMessage> serializer)
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