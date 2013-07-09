#if MONO

using System.Collections;

namespace WampSharp.Tests.TestHelpers
{

    public static class StructuralComparisons
    {
        private static readonly IEqualityComparer mComparer = new StructuralEqualityComparer();

        public static IEqualityComparer StructuralEqualityComparer
        {
            get { return mComparer; }
        }
    }

}

#endif