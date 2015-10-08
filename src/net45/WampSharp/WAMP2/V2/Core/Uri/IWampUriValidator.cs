namespace WampSharp.V2.Core
{
    /// <summary>
    /// Represents a mechanism that checks if uris are valid.
    /// </summary>
    public interface IWampUriValidator
    {
        /// <summary>
        /// Returns a value indicating whether the given uri is a valid uri.
        /// </summary>
        /// <param name="uri">The given uri.</param>
        /// <returns>A value indicating whether the given uri is a valid uri.</returns>
        bool IsValid(string uri);

        /// <summary>
        /// Returns a value indicating whether the given uri is a valid uri 
        /// for the given match type.
        /// </summary>
        /// <param name="uri">The given uri.</param>
        /// <param name="match">The given match type.</param>
        /// <returns>A value indicating whether the given uri is a valid uri 
        /// for the given match type.
        /// </returns>
        bool IsValid(string uri, string match);
    }
}