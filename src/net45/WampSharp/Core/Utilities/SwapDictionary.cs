using System.Collections;
using System.Collections.Generic;

namespace WampSharp.Core.Utilities
{
    internal class SwapDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Members

        private IDictionary<TKey, TValue> mUnderlyingDictionary = new Dictionary<TKey, TValue>();
        private readonly object mLock = new object();

        #endregion

        #region IDictionary methods

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return mUnderlyingDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (mLock)
            {
                IDictionary<TKey, TValue> copy = GetCopy();
                copy.Add(item);
                mUnderlyingDictionary = copy;
            }
        }

        private IDictionary<TKey, TValue> GetCopy()
        {
            return new Dictionary<TKey, TValue>(mUnderlyingDictionary);
        }

        public void Clear()
        {
            lock (mLock)
            {
                mUnderlyingDictionary =
                    new Dictionary<TKey, TValue>();
            }
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
            lock (mLock)
            {
                IDictionary<TKey, TValue> copy = GetCopy();
                bool result = copy.Remove(item);
                mUnderlyingDictionary = copy;
                return result;
            }
        }

        public int Count => mUnderlyingDictionary.Count;

        public bool IsReadOnly => mUnderlyingDictionary.IsReadOnly;

        public bool ContainsKey(TKey key)
        {
            return mUnderlyingDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            lock (mLock)
            {
                IDictionary<TKey, TValue> copy = GetCopy();
                copy.Add(key, value);
                mUnderlyingDictionary = copy;
            }
        }

        public bool Remove(TKey key)
        {
            lock (mLock)
            {
                IDictionary<TKey, TValue> copy = GetCopy();
                bool result = copy.Remove(key);
                mUnderlyingDictionary = copy;
                return result;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return mUnderlyingDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => mUnderlyingDictionary[key];
            set
            {
                lock (mLock)
                {
                    IDictionary<TKey, TValue> copy = GetCopy();
                    copy[key] = value;
                    mUnderlyingDictionary = copy;
                }
            }
        }

        public ICollection<TKey> Keys => mUnderlyingDictionary.Keys;

        public ICollection<TValue> Values => mUnderlyingDictionary.Values;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}