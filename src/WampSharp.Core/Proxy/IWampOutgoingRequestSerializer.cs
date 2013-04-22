using System.Reflection;

namespace WampSharp.Core.Proxy
{
    public interface IWampOutgoingRequestSerializer<TRequest>
    {
        TRequest SerializeRequest(MethodInfo method, object[] arguments);
    }
}