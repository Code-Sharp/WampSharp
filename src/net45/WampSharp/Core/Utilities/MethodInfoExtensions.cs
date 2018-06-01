using System.Reflection;

#if NET40

namespace System
{
    internal static class MethodInfoExtensions
    {
        public static Delegate CreateDelegate(this MethodInfo methodInfo, Type delegateType)
        {
            return Delegate.CreateDelegate(delegateType, methodInfo);
        }
    }
}

#endif