using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.Core.Utilities
{
    internal static class DictionaryExtensions
    {
        public static bool TryRemoveExact<TKey, TValue>
            (this ConcurrentDictionary<TKey, TValue> dictionary,
             TKey key,
             TValue value)
        {
            bool result = false;
            TValue dictionaryValue;

            if (dictionary.TryGetValue(key, out dictionaryValue))
            {
                if (object.ReferenceEquals(dictionaryValue, value))
                {
                    TValue removed;
                    result = dictionary.TryRemove(key, out removed);
                }
            }

            return result;
        }

        public static bool Remove<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> dictionary,
                                                TKey key,
                                                TValue value)
            where TCollection: ICollection<TValue>
        {
            bool result = false;

            TCollection collection;
            
            if (dictionary.TryGetValue(key, out collection))
            {
                if (collection.Remove(value))
                {
                    result = true;

                    if (collection.Count == 0)
                    {
                        result = dictionary.Remove(key);
                    }
                }
            }

            return result;
        }

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