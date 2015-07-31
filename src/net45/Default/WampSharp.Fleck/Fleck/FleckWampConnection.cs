using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Reflection;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : AsyncWampConnection<TMessage>,
        IDetailedWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;
        private readonly byte[] mPingBuffer = new byte[8];
        private readonly TimeSpan mAutoSendPingInterval;
        private readonly FleckTransportDetails mTransportDetails;

        public FleckWampConnection(IWebSocketConnection webSocketConnection, 
            TimeSpan? autoSendPingInterval = null)
        {
            mWebSocketConnection = webSocketConnection;
            mAutoSendPingInterval = autoSendPingInterval ?? TimeSpan.FromSeconds(45);
            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
            mTransportDetails = new FleckTransportDetails(mWebSocketConnection.ConnectionInfo);
        }

        private void OnConnectionOpen()
        {
            RaiseConnectionOpen();

            StartPing();
        }

#if NET40
        private void StartPing()
        {
            Observable.Defer
                (() => Observable.FromAsync
                     (() =>
                     {
                         byte[] ticks = GetCurrentTicks();
                         return mWebSocketConnection.SendPing(ticks);
                     })
                                 .Concat(Observable.Timer(mAutoSendPingInterval)
                                                   .Select(y => Unit.Default))
                )
                      .Repeat()
                      .ToTask()
                      .ContinueWith(x =>
                      {
                          if (x.Exception != null)
                          {
                              mLogger.WarnException("Failed pinging remote peer", x.Exception);
                          }
                      });
        }

#elif NET45
        private void StartPing()
        {
            Task.Run((Action)Ping);
        }

        // We currently detect only disconnected clients and not "zombies",
        // i.e. clients that respond relatively slow.
        private async void Ping()
        {
            while (IsConnected)
            {
                try
                {
                    byte[] ticks = GetCurrentTicks();
                    await mWebSocketConnection.SendPing(ticks);
                    await Task.Delay(mAutoSendPingInterval);
                }
                catch (Exception ex)
                {
                    mLogger.WarnException("Failed pinging remote peer", ex);
                }
            }
        }
#endif

        private byte[] GetCurrentTicks()
        {
            DateTime now = DateTime.Now;
            long ticks = now.Ticks;
            long bytes = ticks;

            for (int i = 0; i < 8; i++)
            {
                mPingBuffer[i] = (byte) bytes;
                bytes = bytes >> 8;
            }

            return mPingBuffer;
        }

        private void OnConnectionError(Exception exception)
        {
            RaiseConnectionError(exception);
        }

        private void OnConnectionClose()
        {
            RaiseConnectionClosed();
        }

        public override void Dispose()
        {
            if (IsConnected)
            {
                mWebSocketConnection.Close();                
            }
        }

        protected override bool IsConnected
        {
            get
            {
                return mWebSocketConnection.IsAvailable;
            }
        }

        public WampTransportDetails TransportDetails
        {
            get
            {
                return mTransportDetails;
            }
        }
    }
}