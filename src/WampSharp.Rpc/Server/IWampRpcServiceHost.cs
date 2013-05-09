namespace WampSharp.Rpc.Server
{
    public interface IWampRpcServiceHost
    {
        void Host(IWampRpcMetadata metadata);

        IWampRpcMethod ResolveMethodByProcUri(string procUri);
    }
}