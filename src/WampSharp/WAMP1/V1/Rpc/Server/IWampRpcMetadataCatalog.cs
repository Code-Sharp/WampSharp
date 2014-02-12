namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// Represents a catalog of hosted rpc services.
    /// </summary>
    public interface IWampRpcMetadataCatalog
    {
        /// <summary>
        /// Registers a new rpc service to the catalog
        /// given its metadata.
        /// </summary>
        /// <param name="metadata">The given service's metadata.</param>
        void Register(IWampRpcMetadata metadata);

        /// <summary>
        /// Maps a given proc uri to its corresponding metadata.
        /// </summary>
        /// <param name="procUri">The given proc uri.</param>
        /// <returns>The corresponding metadata.</returns>
        IWampRpcMethod ResolveMethodByProcUri(string procUri);
    }
}