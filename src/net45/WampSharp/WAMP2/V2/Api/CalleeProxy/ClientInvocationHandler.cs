using System;
#if NET40
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
#endif
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class ClientInvocationHandler : WampCalleeProxyInvocationHandler
    {
        #region Data Members

        private readonly IWampRpcOperationCatalogProxy mCatalogProxy;
        private readonly IWampClientConnectionMonitor mMonitor;

        private TaskCompletionSource<object> mDisconnectionTaskCompletionSource;

        private ManualResetEvent mDisconnectionWaitHandle;

        private Exception mDisconnectionException;
        private readonly CallOptions mEmptyOptions = new CallOptions();

        #endregion

        #region Constructor

        public ClientInvocationHandler(IWampRpcOperationCatalogProxy catalogProxy,
                                       IWampClientConnectionMonitor monitor)
        {
            mCatalogProxy = catalogProxy;
            mMonitor = monitor;
            mDisconnectionTaskCompletionSource = new TaskCompletionSource<object>();
            mDisconnectionWaitHandle = new ManualResetEvent(false);

            mMonitor.ConnectionEstablished += OnConnectionEstablished;
            mMonitor.ConnectionError += OnConnectionError;
            mMonitor.ConnectionBroken += OnConnectionBroken;
        }

        public ClientInvocationHandler(IWampRealmProxy realmProxy) :
            this(realmProxy.RpcCatalog, realmProxy.Monitor)
        {
        }

        #endregion

        #region Private Methods

        private void OnConnectionEstablished(object sender, WampSessionCreatedEventArgs e)
        {
            mDisconnectionTaskCompletionSource = new TaskCompletionSource<object>();
            mDisconnectionWaitHandle = new ManualResetEvent(false);
        }

        private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
        {
            Exception exception = new WampConnectionBrokenException(e);
            SetException(exception);
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            Exception exception = e.Exception;
            SetException(exception);
        }

        private void SetException(Exception exception)
        {
            mDisconnectionException = exception;
            mDisconnectionTaskCompletionSource.TrySetException(exception);
            mDisconnectionWaitHandle.Set();
        }

        #endregion

        #region Overridden

#if NET45
        protected override async Task<T> AwaitForResult<T>(AsyncOperationCallback<T> asyncOperationCallback)
#elif NET40
        protected override Task<T> AwaitForResult<T>(AsyncOperationCallback<T> asyncOperationCallback)
#endif
        {
#if NET45
            Task<T> operationTask = asyncOperationCallback.Task;

            Task task = await Task.WhenAny(operationTask,
                                           mDisconnectionTaskCompletionSource.Task)
                                  .ConfigureAwait(false);

            T result = await operationTask.ConfigureAwait(false);

            return result;
#else
            IObservable<T> merged =
                Observable.Amb(asyncOperationCallback.Task.ToObservable(),
                                    mDisconnectionTaskCompletionSource.Task.ToObservable().Cast<T>());
                
            Task<T> task = merged.ToTask();

            return task;
#endif
        }


        protected override void WaitForResult<T>(SyncCallback<T> callback)
        {
            int signaledIndex =
                WaitHandle.WaitAny(new[] {mDisconnectionWaitHandle, callback.WaitHandle},
                                   Timeout.Infinite);


            if (signaledIndex == 0)
            {
                callback.SetException(mDisconnectionException);
            }

            base.WaitForResult(callback);
        }

        protected override void Invoke(ICalleeProxyInterceptor interceptor, IWampRawRpcOperationClientCallback callback, MethodInfo method, object[] arguments)
        {
            CallOptions callOptions = interceptor.GetCallOptions(method);
            var procedureUri = interceptor.GetProcedureUri(method);

            mCatalogProxy.Invoke(callback,
                                 callOptions,
                                 procedureUri,
                                 arguments);
        }

        #endregion
    }
}