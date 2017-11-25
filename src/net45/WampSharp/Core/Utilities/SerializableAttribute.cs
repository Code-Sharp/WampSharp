#if PCL && !NETSTANDARD2_0

namespace System
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class SerializableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class NonSerializedAttribute : Attribute
    {
    }
}

#endif