using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    public class WampIncompatibleCalleeProxyMethodException : Exception
    {
        public WampIncompatibleCalleeProxyMethodException(MethodInfo method) :
            base(String.Format("Method {0} doesn't have a [WampProcedure] attribute.",
                               method.Name))
        {
        }
    }
}