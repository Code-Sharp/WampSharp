using System.Collections;
using System.Collections.Generic;

namespace WampSharp.Core.Utilities
{
    internal class SwapCollection<T> : ICollection<T>
    {
        private ICollection<T> mCollection = new List<T>();
        private readonly object mLock = new object();

        public void Add(T item)
        {
            lock (mLock)
            {
                var copy = GetCopy();
                copy.Add(item);
                mCollection = copy;
            }
        }

        public void Clear()
        {
            lock (mLock)
            {
                mCollection = GetEmpty();
            }
        }

        private static ICollection<T> GetEmpty()
        {
            return new List<T>();
        }

        private ICollection<T> GetCopy()
        {
            List<T> result = new List<T>(mCollection);
            return result;
        }

        public bool Contains(T item)
        {
            return mCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            mCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            lock (mLock)
            {
                ICollection<T> copy = GetCopy();
                bool result = copy.Remove(item);
                mCollection = copy;
                return result;                
            }
        }

        public int Count => mCollection.Count;

        public bool IsReadOnly => mCollection.IsReadOnly;

        public IEnumerator<T> GetEnumerator()
        {
            return mCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}