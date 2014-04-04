using System.Collections.Concurrent;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Core
{
    internal class WampIdMapper<T>
    {
        private readonly IWampIdGenerator mGenerator = new WampIdGenerator();

        private readonly ConcurrentDictionary<long, T> mIdToValue =
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

        public bool TryRemove(long id, out T value)
        {
            return mIdToValue.TryRemove(id, out value);
        }
    }
}