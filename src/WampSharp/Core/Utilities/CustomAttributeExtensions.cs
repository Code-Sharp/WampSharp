namespace System.Reflection
{
#if !NET45
    public static class CustomAttributeExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit = true) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T), inherit);
        }

        public static T GetCustomAttribute<T>(this ParameterInfo element, bool inherit = true) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T), inherit);
        }
    }
#endif
}