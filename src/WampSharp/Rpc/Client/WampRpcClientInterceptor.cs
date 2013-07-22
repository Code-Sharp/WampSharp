using Castle.DynamicProxy;

namespace WampSharp.Rpc.Client
{
    public abstract class WampRpcClientInterceptor : IInterceptor
    {
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandler mClientHandler;

        public WampRpcClientInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler)
        {
            mSerializer = serializer;
            mClientHandler = clientHandler;
        }

        public IWampRpcSerializer Serializer
        {
            get
            {
                return mSerializer;
            }
        }

        public IWampRpcClientHandler ClientHandler
        {
            get
            {
                return mClientHandler;
            }
        }

        public abstract void Intercept(IInvocation invocation);
    }
}