namespace System.Collections.ObjectModel
{
#if NET40

    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> mUnderlyingDictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> underlyingDictionary)
        {
            mUnderlyingDictionary = new Dictionary<TKey, TValue>(underlyingDictionary);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return mUnderlyingDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return mUnderlyingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            mUnderlyingDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        public int Count
        {
            get
            {
                return mUnderlyingDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return mUnderlyingDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            throw new InvalidOperationException();
        }

        public bool Remove(TKey key)
        {
            throw new InvalidOperationException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return mUnderlyingDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                return mUnderlyingDictionary[key];
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return mUnderlyingDictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return mUnderlyingDictionary.Values;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

#endif
}