 // ReSharper disable once CheckNamespace
namespace System.Reflection
{
#if NET40
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

        public static bool IsDefined(this MemberInfo element, Type attributeType, bool inherit = true)
        {
            return Attribute.IsDefined(element, attributeType, inherit);
        }
    }
#endif
}