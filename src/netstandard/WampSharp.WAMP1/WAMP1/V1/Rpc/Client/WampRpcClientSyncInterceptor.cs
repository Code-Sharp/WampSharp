using System.Reflection;

namespace WampSharp.V1.Rpc.Client
{
    internal class WampRpcClientSyncInterceptor : WampRpcClientInterceptor
    {
        public WampRpcClientSyncInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler) : base(serializer, clientHandler)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            WampRpcCall wampRpcCall = Serializer.Serialize(method, arguments);
            object result = ClientHandler.Handle(wampRpcCall);

            return result;
        }
    }
}