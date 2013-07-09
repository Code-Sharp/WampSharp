#if MONO

using System.Collections;
using System.Collections.Generic;

namespace WampSharp.Tests.TestHelpers
{
    internal class StructuralEqualityComparer : IEqualityComparer<IStructuralEquatable>,
                                                IEqualityComparer
    {
        public bool Equals(IStructuralEquatable x, IStructuralEquatable y)
        {
            if (x == null)
            {
                return (y == null);
            }

            return x.Equals(y, EqualityComparer<object>.Default);
        }

        public int GetHashCode(IStructuralEquatable obj)
        {
            return obj.GetHashCode(EqualityComparer<object>.Default);
        }

        public bool Equals(object x, object y)
        {
            return this.Equals(x as IStructuralEquatable,
                               y as IStructuralEquatable);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(obj as IStructuralEquatable);
        }
    }
}

#endif