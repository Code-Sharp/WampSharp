using System.Reflection;

namespace WampSharp.CodeGeneration
{
    internal interface IProxyMethodWriter
    {
        string WriteField(int methodIndex, MethodInfo method);
        string WriteMethod(int methodIndex, MethodInfo method);
    }
}