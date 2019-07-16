using System;
using System.Threading;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Message;
using WampSharp.Logging;

namespace WampSharp.Core.Listener
{
    public abstract class AsyncWampConnection<TMessage> : IWampConnection<TMessage>,
        IAsyncDisposable
    {
        private readonly ActionBlock<WampMessage<object>> mSendBlock;
        protected readonly ILog mLogger;
        private int mDisposeCalled = 0;

        protected AsyncWampConnection()
        {
            mLogger = new LoggerWithConnectionId(LogProvider.GetLogger(this.GetType()));
            mSendBlock = new ActionBlock<WampMessage<object>>(x => InnerSend(x));
        }

        public void Send(WampMessage<object> message)
        {
            mSendBlock.Post(message);
        }

#if ASYNC

        protected async Task InnerSend(WampMessage<object> message)
        {
            const string errorMessage = 
                "An error occured while attempting to send a message to remote peer.";

            if (IsConnected)
            {
                try
                {
                    Task sendAsync = SendAsync(message);

                    if (sendAsync != null)
                    {
                        await sendAsync.ConfigureAwait(false);
                    }
                    else
                    {
                        mLogger.Error(errorMessage + " Got null Task.");
                    }
                }
                catch (Exception ex)
                {
                    mLogger.Error(errorMessage, ex);
                }
            }
        }

#else
        protected Task InnerSend(WampMessage<object> message)
        {
            const string errorMessage =
                "An error occured while attempting to send a message to remote peer.";

            if (IsConnected)
            {
                Task sendAsync = SendAsync(message);

                if (sendAsync == null)
                {
                    mLogger.Error(errorMessage + " Got null Task.");
                }
                else
                {
                    Task result = sendAsync.ContinueWith(task =>
                    {
                        var ex = task.Exception;

                        if (ex != null)
                        {
                            mLogger.Error(errorMessage, ex);
                        }
                    });

                    return result;
                }
            }

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

#endif

        protected abstract bool IsConnected { get; }

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
        protected abstract Task SendAsync(WampMessage<object> message);

        protected virtual void RaiseConnectionOpen()
        {
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            MessageArrived?.Invoke(this, new WampMessageArrivedEventArgs<TMessage>(message));
        }

        protected virtual void RaiseConnectionClosed()
        {
            mLogger.Debug("Connection has been closed");
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionError(Exception ex)
        {
            mLogger.Error("A connection error occured", ex);
            ConnectionError?.Invoke(this, new WampConnectionErrorEventArgs(ex));
        }
        
        void IDisposable.Dispose()
        {
            if (Interlocked.CompareExchange(ref mDisposeCalled, 1, 0) == 0)
            {
                mSendBlock.Complete();
                mSendBlock.Completion.Wait();
                this.Dispose();
            }
        }

        protected abstract void Dispose();

#if ASYNC

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (Interlocked.CompareExchange(ref mDisposeCalled, 1, 0) == 0)
            {
                mSendBlock.Complete();
                await mSendBlock.Completion.ConfigureAwait(false);
                this.Dispose();
            }
        }

#else

        Task IAsyncDisposable.DisposeAsync()
        {
            mSendBlock.Complete();
            return mSendBlock.Completion.ContinueWith(x => x.Dispose());
        }

#endif

        // TODO: move this to another file (after making it more generic)
        // TODO: or get rid of this.
        private class LoggerWithConnectionId : ILog
        {
            private readonly ILog mLogger;
            private readonly string mConnectionId;

            public LoggerWithConnectionId(ILog logger)
            {
                mConnectionId = Guid.NewGuid().ToString();
                mLogger = logger;
            }

            public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
            {
                using (LogProvider.OpenMappedContext("ConnectionId", mConnectionId))
                {
                    return mLogger.Log(logLevel, messageFunc, exception, formatParameters);
                }
            }
        }
    }
}