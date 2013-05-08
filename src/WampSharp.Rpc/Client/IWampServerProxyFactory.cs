using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;

namespace WampSharp.Rpc
{
    public interface IWampServerProxyFactory<TMessage>
    {
        IWampServer Create(IWampClient<TMessage> client);
    }
}