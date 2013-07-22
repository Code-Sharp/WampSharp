using System.Reflection;

namespace WampSharp.Rpc.Client
{
    public interface IWampRpcSerializer<TMessage>
    {
        WampRpcCall<TMessage> Serialize(MethodInfo method, object[] arguments);
    }

    public interface IWampRpcSerializer : IWampRpcSerializer<object>
    {
    }
}