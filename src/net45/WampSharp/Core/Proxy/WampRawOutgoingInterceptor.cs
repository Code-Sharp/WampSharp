using Castle.DynamicProxy;
using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    public class WampRawOutgoingInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingMessageHandler<TMessage> mOutgoingHandler;

        public WampRawOutgoingInterceptor(IWampOutgoingMessageHandler<TMessage> outgoingHandler)
        {
            mOutgoingHandler = outgoingHandler;
        }

        public void Intercept(IInvocation invocation)
        {
            WampMessage<TMessage> message = invocation.Arguments[0] as WampMessage<TMessage>;
            mOutgoingHandler.Handle(message);
        }
    }
}