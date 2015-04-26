using Castle.DynamicProxy;

namespace WampSharp.V1.Rpc.Client
{
    internal class WampRpcClientSyncInterceptor : WampRpcClientInterceptor
    {
        public WampRpcClientSyncInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler) : base(serializer, clientHandler)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            WampRpcCall wampRpcCall = Serializer.Serialize(invocation.Method, invocation.Arguments);
            object result = ClientHandler.Handle(wampRpcCall);

            invocation.ReturnValue = result;
        }
    }
}