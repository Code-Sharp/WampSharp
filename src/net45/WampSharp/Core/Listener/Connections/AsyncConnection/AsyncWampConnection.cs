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
            mLogger.Debug("Connection has been closed");
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
            if (Interlocked.CompareExchange(ref mDisposeCalled, 1, 0) == 0)
            {
                mSendBlock.Complete();
                mSendBlock.Completion.Wait();
                this.Dispose();
            }
        }

        protected abstract void Dispose();

        async Task IAsyncDisposable.DisposeAsync()
        {
            if (Interlocked.CompareExchange(ref mDisposeCalled, 1, 0) == 0)
            {
                mSendBlock.Complete();
                await mSendBlock.Completion;
                this.Dispose();
            }
        }
        

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
                using (LogProvider.OpenMappedContext("ConncetionId", mConnectionId))
                {
                    return mLogger.Log(logLevel, messageFunc, exception, formatParameters);
                }
            }
        }
    }
}