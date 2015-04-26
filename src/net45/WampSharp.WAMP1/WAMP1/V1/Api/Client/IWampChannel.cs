using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V1.Auxiliary.Client;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1
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

        void MapPrefix(string prefix, string uri);

        IWampClientConnectionMonitor GetMonitor();

        void Open();

        Task OpenAsync();

        void Close();
    }
}