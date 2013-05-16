using System.Reflection;

namespace WampSharp.Rpc
{
    public interface IWampProcUriMapper
    {
        string Map(MethodInfo method);
    }
}