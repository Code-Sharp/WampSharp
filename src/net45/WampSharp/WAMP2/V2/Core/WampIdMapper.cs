using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.V2.Core
{
    internal class WampIdMapper<T>
    {
        private readonly IWampIdGenerator mGenerator = new WampIdGenerator();

        protected readonly ConcurrentDictionary<long, T> mIdToValue =
            new ConcurrentDictionary<long, T>();
         
        public long Add(T value)
        {
            bool added = false;
            long currentId = default(long); // Has no meaning.

            while (!added)
            {
                currentId = mGenerator.Generate();

                added = mIdToValue.TryAdd(currentId, value);                
            }

            return currentId;
        }

        public bool TryGetValue(long key, out T value)
        {
            return mIdToValue.TryGetValue(key, out value);
        }

        public bool TryRemove(long id, out T value)
        {
            return mIdToValue.TryRemove(id, out value);
        }
    }
}