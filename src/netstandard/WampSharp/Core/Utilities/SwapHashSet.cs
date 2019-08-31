using System;
using System.Collections;
using System.Collections.Generic;

namespace WampSharp.Core.Utilities
{
    internal class SwapHashSet<T> : ISet<T>
    {
        private HashSet<T> mHashSet;

        private readonly object mLock = new object();

        public SwapHashSet()
        {
            mHashSet = new HashSet<T>();
        }

        public SwapHashSet(IEqualityComparer<T> comparer)
        {
            mHashSet = new HashSet<T>(comparer);
        }

        public SwapHashSet(IEnumerable<T> collection)
        {
            mHashSet = new HashSet<T>(collection);
        }

        public SwapHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            mHashSet = new HashSet<T>(collection, comparer);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            lock (mLock)
            {
                mHashSet = new HashSet<T>(mHashSet.Comparer);
            }
        }

        public bool Contains(T item)
        {
            return mHashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            mHashSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            lock (mLock)
            {
                HashSet<T> copy = GetCopy();
                bool removed = copy.Remove(item);
                mHashSet = copy;
                return removed;
            }
        }

        private HashSet<T> GetCopy()
        {
            return new HashSet<T>(mHashSet, mHashSet.Comparer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return mHashSet.GetEnumerator();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            lock (mLock)
            {
                HashSet<T> copy = GetCopy();
                copy.UnionWith(other);
                mHashSet = copy;
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            lock (mHashSet)
            {
                HashSet<T> copy = GetCopy();
                copy.IntersectWith(other);
                mHashSet = copy;
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (mHashSet)
            {
                HashSet<T> copy = GetCopy();
                copy.ExceptWith(other);
                mHashSet = copy;
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (mHashSet)
            {
                HashSet<T> copy = GetCopy();
                copy.SymmetricExceptWith(other);
                mHashSet = copy;
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return mHashSet.IsSubsetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return mHashSet.IsProperSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return mHashSet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return mHashSet.IsProperSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return mHashSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return mHashSet.SetEquals(other);
        }

        public void CopyTo(T[] array)
        {
            mHashSet.CopyTo(array);
        }

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            mHashSet.CopyTo(array, arrayIndex, count);
        }

        public int RemoveWhere(Predicate<T> match)
        {
            lock (mHashSet)
            {
                HashSet<T> copy = GetCopy();
                int result = copy.RemoveWhere(match);
                mHashSet = copy;
                return result;
            }
        }

        public void TrimExcess()
        {
            lock (mHashSet)
            {
                HashSet<T> copy = GetCopy();
                copy.TrimExcess();
                mHashSet = copy;
            }
        }

        public int Count => mHashSet.Count;

        public IEqualityComparer<T> Comparer => mHashSet.Comparer;

        public bool Add(T item)
        {
            lock (mLock)
            {
                HashSet<T> copy = GetCopy();
                var result = copy.Add(item);
                mHashSet = copy;
                return result;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                ICollection<T> hashSet = mHashSet;
                return hashSet.IsReadOnly;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}