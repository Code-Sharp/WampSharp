using System.Collections.Generic;

namespace WampSharp.Core.Dispatch.Handler
{
    internal static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dictionary,
                                             TKey key,
                                             TValue value)
        {
            dictionary.Add<TKey, TValue, ICollection<TValue>, List<TValue>>(key, value);
        }

        public static void Add<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> dictionary,
                                                          TKey key,
                                                          TValue value)
            where TCollection : ICollection<TValue>, new()
        {
            dictionary.Add<TKey, TValue, TCollection, TCollection>(key, value);
        }

        private static void Add<TKey, TValue, TCollection, TNewCollection>(this IDictionary<TKey, TCollection> dictionary,
                                                          TKey key,
                                                          TValue value)
            where TCollection : ICollection<TValue>
            where TNewCollection : TCollection, new()
        {
            TCollection collection;

            if (!dictionary.TryGetValue(key, out collection))
            {
                collection = new TNewCollection();
                dictionary[key] = collection;
            }

            collection.Add(value);
        }
    }
}