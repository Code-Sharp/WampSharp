using System;
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

        private TaskCompletionSource<Exception> mDisconnectionTaskCompletionSource;

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

            mDisconnectionTaskCompletionSource = new TaskCompletionSource<Exception>();
            mDisconnectionWaitHandle = new ManualResetEvent(false);

            mMonitor.ConnectionError += OnConnectionError;
            mMonitor.ConnectionBroken += OnConnectionBroken;
        }

        public ClientInvocationHandler(IWampRealmProxy realmProxy) :
            this(realmProxy.RpcCatalog, realmProxy.Monitor)
        {
        }

        #endregion

        #region Private Methods


        private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
        {
            Exception exception = new WampConnectionBrokenException(e);
            SetException(exception);

            mDisconnectionTaskCompletionSource = new TaskCompletionSource<Exception>();
            mDisconnectionWaitHandle = new ManualResetEvent(false);
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            Exception exception = e.Exception;
            SetException(exception);
        }

        private void SetException(Exception exception)
        {
            mDisconnectionException = exception;
            mDisconnectionTaskCompletionSource.TrySetResult(exception);
            mDisconnectionWaitHandle.Set();
        }

        #endregion

        #region Overridden

        protected override async Task<T> AwaitForResult<T>(AsyncOperationCallback<T> asyncOperationCallback, CancellationTokenRegistration registration)
        {
            Task<T> operationTask = asyncOperationCallback.Task;

            Task<Exception> disconnectionTask = mDisconnectionTaskCompletionSource.Task;

            Task task = await Task.WhenAny(operationTask,
                                           disconnectionTask)
                                  .ConfigureAwait(false);

            registration.Dispose();

            if (!operationTask.IsCompleted)
            {
                Exception exception = await disconnectionTask.ConfigureAwait(false);

                asyncOperationCallback.SetException(exception);
            }

            T result = await operationTask.ConfigureAwait(false);

            return result;
        }


        protected override void WaitForResult<T>(SyncCallback<T> callback)
        {
            int signaledIndex =
                WaitHandle.WaitAny(new[] { mDisconnectionWaitHandle, callback.WaitHandle },
                                   Timeout.Infinite);


            if (signaledIndex == 0)
            {
                callback.SetException(mDisconnectionException);
            }

            base.WaitForResult(callback);
        }

        protected override IWampCancellableInvocationProxy Invoke(ICalleeProxyInterceptor interceptor, IWampRawRpcOperationClientCallback callback, MethodInfo method, object[] arguments)
        {
            CallOptions callOptions = interceptor.GetCallOptions(method);
            var procedureUri = interceptor.GetProcedureUri(method);

            return mCatalogProxy.Invoke(callback,
                                        callOptions,
                                        procedureUri,
                                        arguments);
        }

#endregion
    }
}