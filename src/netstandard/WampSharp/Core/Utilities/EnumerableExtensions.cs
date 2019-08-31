using System.Collections.Generic;
using System.Linq;

namespace WampSharp.Core.Utilities
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(Yield(item));
        }

        private static IEnumerable<T> Yield<T>(T item)
        {
            yield return item;
        }
    }
}