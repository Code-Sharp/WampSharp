using System.Reflection;

namespace WampSharp.Core.Utilities
{
    internal static class ParameterInfoExtensions
    {
        public static bool HasDefaultValue(this ParameterInfo parameter)
        {
            return parameter.HasDefaultValue;
        }
    }
}