using System.Reactive.Subjects;
using WampSharp.Core.Contracts.V1;

namespace WampSharp.Api
{
    public interface IWampChannel<TMessage>
    {
        IWampServer GetServerProxy(IWampClient<TMessage> callbackClient);

        TProxy GetRpcProxy<TProxy>() where TProxy : class;

        dynamic GetDynamicRpcProxy();

        // TODO: the ISubject interface doesn't allow to use some pubsub features
        // TODO: such as publish excluded/eligible and etc.
        // TODO: consider replacing it with a different interface.
        ISubject<T> GetSubject<T>(string topicUri);         
    }
}