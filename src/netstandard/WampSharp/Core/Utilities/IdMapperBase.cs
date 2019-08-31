using System.Collections.Concurrent;

namespace WampSharp.Core.Utilities
{
    internal abstract class IdMapperBase<TKey, T>
    {
        protected readonly ConcurrentDictionary<TKey, T> mIdToValue =
            new ConcurrentDictionary<TKey, T>();

        public TKey Add(T value)
        {
            bool added = false;
            TKey currentId = default(TKey); // Has no meaning.

            while (!added)
            {
                currentId = GenerateId();

                added = mIdToValue.TryAdd(currentId, value);                
            }

            return currentId;
        }

        protected abstract TKey GenerateId();

        public bool TryGetValue(TKey key, out T value)
        {
            return mIdToValue.TryGetValue(key, out value);
        }

        public bool TryRemove(TKey id, out T value)
        {
            return mIdToValue.TryRemove(id, out value);
        }

        public bool TryRemoveExact(TKey id, T value)
        {
            return mIdToValue.TryRemoveExact(id, value);
        }

        public void Clear()
        {
            mIdToValue.Clear();
        }
    }
}