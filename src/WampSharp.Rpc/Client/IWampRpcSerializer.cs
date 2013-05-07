using System.Reflection;

namespace WampSharp.Rpc
{
    public interface IWampRpcSerializer<TMessage>
    {
        WampRpcCall<TMessage> Serialize(MethodInfo method, object[] arguments);
    }


    public interface IWampRpcSerializer : IWampRpcSerializer<object>
    {
    }
}