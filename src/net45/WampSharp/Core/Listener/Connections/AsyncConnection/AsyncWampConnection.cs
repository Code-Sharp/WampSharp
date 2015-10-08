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

        protected AsyncWampConnection()
        {
            mLogger = LogProvider.GetLogger(this.GetType());
            mSendBlock = new ActionBlock<WampMessage<object>>(x => InnerSend(x));
        }

        public void Send(WampMessage<object> message)
        {
            mSendBlock.Post(message);
        }

#if !NET40

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
            var handler = ConnectionOpen;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            var handler = MessageArrived;
            if (handler != null) handler(this, new WampMessageArrivedEventArgs<TMessage>(message));
        }

        protected virtual void RaiseConnectionClosed()
        {
            var handler = ConnectionClosed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionError(Exception ex)
        {
            mLogger.Error("A connection error occured", ex);
            var handler = ConnectionError;
            if (handler != null) handler(this, new WampConnectionErrorEventArgs(ex));
        }
        
        void IDisposable.Dispose()
        {
            mSendBlock.Complete();
            mSendBlock.Completion.Wait();
            this.Dispose();
        }

        protected abstract void Dispose();

#if NET45

        async Task IAsyncDisposable.DisposeAsync()
        {
            mSendBlock.Complete();
            await mSendBlock.Completion;
            this.Dispose();
        }

#elif NET40

        Task IAsyncDisposable.DisposeAsync()
        {
            mSendBlock.Complete();
            return mSendBlock.Completion.ContinueWith(x => x.Dispose());
        }

#endif

    }
}