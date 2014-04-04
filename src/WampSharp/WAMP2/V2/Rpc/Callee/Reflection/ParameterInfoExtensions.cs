using System.Reflection;

namespace WampSharp.V2.Rpc
{
    internal static class ParameterInfoExtensions
    {     
        public static bool HasDefaultValue(this ParameterInfo parameter)
        {
#if NET45
            return parameter.HasDefaultValue;
#elif NET40
            return (parameter.Attributes & ParameterAttributes.HasDefault) == 
                   ParameterAttributes.HasDefault;
#endif
        }
    }
}