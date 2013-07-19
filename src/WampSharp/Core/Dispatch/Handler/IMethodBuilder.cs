namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// Builds a method by a given key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    public interface IMethodBuilder<TKey, TMethod>
    {
        /// <summary>
        /// Builds a method by a given key.
        /// </summary>
        /// <returns>A delegate to the given method.</returns>
        TMethod BuildMethod(TKey key);
    }
}