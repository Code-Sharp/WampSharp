using System;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Client;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : WampCalleeProxyFactory
    {
        public WampCalleeClientProxyFactory(IWampRpcOperationCatalogProxy catalogProxy, IWampClientConnectionMonitor monitor) : 
            base(new ClientInvocationHandler(catalogProxy, monitor))
        {
        }

        private class ClientInvocationHandler : WampCalleeProxyInvocationHandler
        {
            #region Data Members

            private readonly IWampRpcOperationCatalogProxy mCatalogProxy;
            private readonly IWampClientConnectionMonitor mMonitor;

            private readonly TaskCompletionSource<object> mDisconnectionTaskCompletionSource =
                new TaskCompletionSource<object>();
            
            private readonly ManualResetEvent mDisconnectionWaitHandle =
                new ManualResetEvent(false);

            private Exception mDisconnectionException;

            #endregion

            #region Constructor

            public ClientInvocationHandler(IWampRpcOperationCatalogProxy catalogProxy,
                IWampClientConnectionMonitor monitor)
            {
                mCatalogProxy = catalogProxy;
                mMonitor = monitor;

                mMonitor.ConnectionError += OnConnectionError;
                mMonitor.ConnectionBroken += OnConnectionBroken;
            }

            #endregion

            #region Private Methods

            private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
            {
                Exception exception = new WampConnectionBrokenException();
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
                mDisconnectionTaskCompletionSource.SetException(exception);
                mDisconnectionWaitHandle.Set();
            }

            #endregion

            #region Overridden

            protected async override Task<object> AwaitForResult(AsyncOperationCallback asyncOperationCallback)
            {
#if NET45
                return await
                       Task.WhenAny(asyncOperationCallback.Task,
                                    mDisconnectionTaskCompletionSource.Task);
#else
                // TODO: Add framework 4 implementation.
                return asyncOperationCallback.Task;
#endif
            }

            protected override void WaitForResult(SyncCallback callback)
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

            protected override void Invoke(IWampRawRpcOperationCallback callback, string procedure, object[] arguments)
            {
                mCatalogProxy.Invoke(callback,
                                     mEmptyOptions,
                                     procedure,
                                     arguments);
            }

            #endregion
        }
    }
}