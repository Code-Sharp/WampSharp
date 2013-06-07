namespace System.Reflection
{
#if !NET45
    public static class CustomAttributeExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T), inherit);
        }
    }
#endif
}