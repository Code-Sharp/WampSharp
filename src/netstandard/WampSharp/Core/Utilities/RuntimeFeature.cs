#if !NETSTANDARD2_1

namespace System.Runtime.CompilerServices
{
    internal class RuntimeFeature
    {
        public const bool IsDynamicCodeCompiled = true;
        public const bool IsDynamicCodeSupported = true;
    }
}

#endif