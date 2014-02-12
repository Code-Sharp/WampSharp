namespace WampSharp.V1.Core.Curie
{
    /// <summary>
    /// Maps prefixes to uris.
    /// </summary>
    public interface IWampCurieMapper
    {
        /// <summary>
        /// Resolves a compact uri by the current prefixes mapping.
        /// </summary>
        /// <param name="curie">The given compact uri.</param>
        /// <returns>The resolved uri.</returns>
        string Resolve(string curie);
        
        /// <summary>
        /// Maps a prefix to a uri.
        /// </summary>
        /// <param name="prefix">The given prefix.</param>
        /// <param name="uri">The given uri.</param>
        void Map(string prefix, string uri);
    }
}