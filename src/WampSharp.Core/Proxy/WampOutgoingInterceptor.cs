using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    public class WampOutgoingInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingMessageHandler<TMessage> mOutgoingHandler;
        private readonly IWampOutgoingRequestSerializer<WampMessage<TMessage>> mOutgoingSerializer;

        public WampOutgoingInterceptor(IWampOutgoingRequestSerializer<WampMessage<TMessage>> outgoingSerializer,
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

            IWampClient client = null;

            if (invocation.Arguments.Length >= 0)
            {
                client = invocation.Arguments[0] as IWampClient;
            }

            mOutgoingHandler.Handle(client, serialized);
        }
    }
}
