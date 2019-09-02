using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V1.Auxiliary.Client;
using WampSharp.V1.Core.Client;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.PubSub.Client;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.V1
{
    public class WampChannel<TMessage> : IWampChannel<TMessage>
    {
        private readonly IControlledWampConnection<TMessage> mConnection;
        private readonly IWampRpcClientFactory<TMessage> mRpcClientFactory;
        private readonly IWampPubSubClientFactory<TMessage> mPubSubClientFactory;
        private readonly ManualWampServerProxyBuilder<TMessage, IWampClient<TMessage>> mServerProxyBuilder;
        private readonly IWampClientConnectionMonitor mConnectionMonitor;

        public WampChannel(IControlledWampConnection<TMessage> connection,
                           IWampRpcClientFactory<TMessage> rpcClientFactory,
                           IWampPubSubClientFactory<TMessage> pubSubClientFactory,
                           ManualWampServerProxyBuilder<TMessage, IWampClient<TMessage>> serverProxyBuilder,
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

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public Task OpenAsync()
        {
            var connectedObservable =
                Observable.FromEventPattern<WampConnectionEstablishedEventArgs>
                    (x => mConnectionMonitor.ConnectionEstablished += x,
                     x => mConnectionMonitor.ConnectionEstablished -= x)
                          .Select(x => Unit.Default);

            var errorObservable =
                Observable.FromEventPattern<WampConnectionErrorEventArgs>
                    (x => mConnectionMonitor.ConnectionError += x,
                     x => mConnectionMonitor.ConnectionError -= x)
                          .Select(x => Observable.Throw<Unit>(x.EventArgs.Exception))
                          .SelectMany(x => x);

            var completedObservable =
                Observable.FromEventPattern
                    (x => mConnectionMonitor.ConnectionLost += x,
                     x => mConnectionMonitor.ConnectionLost -= x)
                     .Select(x => Unit.Default);

            // Combining the observables and propagating the one that reatcs first
            // because we have to complete the task either when a connection is established or
            // an error (i.e. exception) occurs.
            var combined = connectedObservable.Amb(errorObservable).Amb(completedObservable);

            var firstConnectionOrError =
                combined.Take(1);

            Task task = firstConnectionOrError.ToTask();

            mConnection.Connect();

            return task;
        }

        public void Close()
        {
            mConnection.Dispose();
        }
    }
}