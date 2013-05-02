using Castle.DynamicProxy;

namespace WampSharp.Rpc
{
    class WampRpcClientInterceptor : IInterceptor
    {
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandler mClientHandler;

        public WampRpcClientInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler)
        {
            mSerializer = serializer;
            mClientHandler = clientHandler;
        }

        public void Intercept(IInvocation invocation)
        {
            var wampRpcCall = mSerializer.Serialize(invocation.Method, invocation.Arguments);
            var result = mClientHandler.Handle(wampRpcCall);

            invocation.ReturnValue = result;
        }
    }
}