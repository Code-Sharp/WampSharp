namespace WampSharp.Rpc.Server
{
    public interface IWampRpcMetadataCatalog
    {
        void Register(IWampRpcMetadata metadata);

        IWampRpcMethod ResolveMethodByProcUri(string procUri);
    }
}