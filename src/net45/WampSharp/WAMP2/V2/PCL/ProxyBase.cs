#if PCL
using System.Reflection;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.V2
{
    internal class ProxyBase
    {
        private readonly IWampOutgoingMessageHandler mMessageHandler;
        private readonly IWampOutgoingRequestSerializer mRequestSerializer;

        protected ProxyBase(IWampOutgoingMessageHandler messageHandler, IWampOutgoingRequestSerializer requestSerializer)
        {
            mMessageHandler = messageHandler;
            mRequestSerializer = requestSerializer;
        }

        protected void Send(MethodInfo method, params object[] arguments)
        {
            WampMessage<object> serialized = mRequestSerializer.SerializeRequest(method, arguments);
            mMessageHandler.Handle(serialized);
        }

        protected void Send(WampMessage<object> message)
        {
            mMessageHandler.Handle(message);
        }
    }
}
#endif