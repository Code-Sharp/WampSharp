using System.Collections.Immutable;
using System.Threading;

namespace WampSharp.Core.Utilities
{
    internal static class ImmutableHashSetInterlocked
    {
        public static void Add<T>(ref ImmutableHashSet<T> hashSet, T item)
        {
            ImmutableHashSet<T> previousValue;
            ImmutableHashSet<T> valueBeforeChange = Volatile.Read(ref hashSet);

            do
            {
                previousValue = valueBeforeChange;
                ImmutableHashSet<T> modified = previousValue.Add(item);
                valueBeforeChange = Interlocked.CompareExchange(ref hashSet, modified, previousValue);
            }
            while (valueBeforeChange != previousValue);
        }

        public static void Remove<T>(ref ImmutableHashSet<T> hashSet, T item)
        {
            ImmutableHashSet<T> previousValue;
            ImmutableHashSet<T> valueBeforeChange = Volatile.Read(ref hashSet);

            do
            {
                previousValue = valueBeforeChange;
                ImmutableHashSet<T> modified = previousValue.Remove(item);
                valueBeforeChange = Interlocked.CompareExchange(ref hashSet, modified, previousValue);
            }
            while (valueBeforeChange != previousValue);
        }
    }
}