using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.V1.Auxiliary.Client;
using WampSharp.V1.Core.Contracts.V1;
using WampSharp.V1.PubSub.Client;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.V1
{
    public class WampChannel<TMessage> : IWampChannel<TMessage>
    {
        private readonly IControlledWampConnection<TMessage> mConnection;
        private readonly IWampRpcClientFactory<TMessage> mRpcClientFactory;
        private readonly IWampPubSubClientFactory<TMessage> mPubSubClientFactory;
        private readonly WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> mServerProxyBuilder;
        private readonly IWampClientConnectionMonitor mConnectionMonitor;

        public WampChannel(IControlledWampConnection<TMessage> connection,
                           IWampRpcClientFactory<TMessage> rpcClientFactory,
                           IWampPubSubClientFactory<TMessage> pubSubClientFactory,
                           WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> serverProxyBuilder,
                           IWampAuxiliaryClientFactory<TMessage> connectionMonitorFactory)
        {
            mConnection = connection;
            mRpcClientFactory = rpcClientFactory;
            mPubSubClientFactory = pubSubClientFactory;
            mServerProxyBuilder = serverProxyBuilder;
            mConnectionMonitor = connectionMonitorFactory.CreateMonitor(connection);
        }

        public IWampServer GetServerProxy(IWampClient<TMessage> callbackClient)
        {
            return mServerProxyBuilder.Create(callbackClient, mConnection);
        }

        public TProxy GetRpcProxy<TProxy>() where TProxy : class
        {
            return mRpcClientFactory.GetClient<TProxy>(mConnection);
        }

        public dynamic GetDynamicRpcProxy()
        {
            return mRpcClientFactory.GetDynamicClient(mConnection);
        }

        public ISubject<T> GetSubject<T>(string topicUri)
        {
            return mPubSubClientFactory.GetSubject<T>(topicUri, mConnection);
        }

        public void MapPrefix(string prefix, string uri)
        {
            mConnectionMonitor.MapPrefix(prefix, uri);
        }

        public IWampClientConnectionMonitor GetMonitor()
        {
            return mConnectionMonitor;
        }

        public void Open()
        {
            Task task = OpenAsync();
            task.Wait();
        }

        public Task OpenAsync()
        {
            var observable =
                Observable.FromEventPattern<WampConnectionEstablishedEventArgs>
                    (x => mConnectionMonitor.ConnectionEstablished += x,
                     x => mConnectionMonitor.ConnectionEstablished -= x)
                          .Select(x => Unit.Default);

            IObservable<Unit> firstConnection =
                observable.Take(1);

            Task task = firstConnection.ToTask();

            mConnection.Connect();
            
            return task;
        }

        public void Close()
        {
            mConnection.OnCompleted();
        }
    }
}