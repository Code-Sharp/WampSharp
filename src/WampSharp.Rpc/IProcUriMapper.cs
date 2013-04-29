using System.Reflection;

namespace WampSharp.Rpc
{
    public interface IProcUriMapper
    {
        string Map(MethodInfo method);
    }
}