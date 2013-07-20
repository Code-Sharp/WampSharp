using System.Reflection;

namespace WampSharp.Rpc.Client
{
    public interface IWampProcUriMapper
    {
        string Map(MethodInfo method);
    }
}