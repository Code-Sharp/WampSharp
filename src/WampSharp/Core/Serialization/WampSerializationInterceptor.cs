using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Serialization
{
    public class WampSerializationInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingRequestSerializer<TMessage> mSerializer;

        public WampSerializationInterceptor(IWampOutgoingRequestSerializer<TMessage> serializer)
        {
            mSerializer = serializer;
        }

        public void Intercept(IInvocation invocation)
        {
            WampMessage<TMessage> result =
                mSerializer.SerializeRequest(invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}