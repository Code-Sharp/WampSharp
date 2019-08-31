using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    public class WampIncompatibleCalleeProxyMethodException : Exception
    {
        public WampIncompatibleCalleeProxyMethodException(MethodInfo method) :
            base($"Method {method.Name} doesn't have a [WampProcedure] attribute.")
        {
        }
    }
}