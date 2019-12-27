using System.Collections;
using System.Collections.Generic;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRawComparer : IEqualityComparer, IEqualityComparer<MockRaw>
    {
        public new bool Equals(object x, object y)
        {
            return Equals(x as MockRaw, y as MockRaw);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(obj as MockRaw);
        }

        public bool Equals(MockRaw x, MockRaw y)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode(MockRaw obj)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
        }
    }
}