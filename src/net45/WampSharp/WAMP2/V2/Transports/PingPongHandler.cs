using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Logging;

namespace WampSharp.V2.Transports
{
    public class PingPongHandler
    {
        private readonly byte[] mPingBuffer = new byte[8];
        private readonly ILog mLogger;
        private readonly TimeSpan? mAutoSendPingInterval;
        private readonly IPinger mPinger;

        public PingPongHandler(ILog logger, IPinger pinger, TimeSpan? autoSendPingInterval)
        {
            mLogger = logger;
            mAutoSendPingInterval = autoSendPingInterval;
            mPinger = pinger;
            mPinger.OnPong += OnPong;
        }

        public void Start()
        {
            if (mAutoSendPingInterval != null)
            {
                StartPing();
            }
        }

        private void OnPong(IList<byte> collection)
        {
            // TODO: Check that the time between this value to DateTime.Now
            // TODO: does not exceed a given value.
            // TODO: If it does, disconnect the client.
        }


#if ASYNC
        private void StartPing()
        {
            Task.Run((Action)Ping);
        }

        // We currently detect only disconnected clients and not "zombies",
        // i.e. clients that respond relatively slow.
        private async void Ping()
        {
            while (mPinger.IsConnected)
            {
                try
                {
                    byte[] ticks = GetCurrentTicks();
                    await mPinger.SendPing(ticks).ConfigureAwait(false);
                    await Task.Delay(mAutoSendPingInterval.Value).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    mLogger.WarnException("Failed pinging remote peer", ex);
                }
            }
        }
#else
        private void StartPing()
        {
            Observable.Defer
                (() => Observable.FromAsync
                     (() =>
                     {
                         byte[] ticks = GetCurrentTicks();
                         return mPinger.SendPing(ticks);
                     })
                                 .Concat(Observable.Timer(mAutoSendPingInterval.Value)
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
#endif

        private byte[] GetCurrentTicks()
        {
            DateTime now = DateTime.Now;
            long ticks = now.Ticks;
            long bytes = ticks;

            for (int i = 0; i < 8; i++)
            {
                mPingBuffer[i] = (byte)bytes;
                bytes = bytes >> 8;
            }

            return mPingBuffer;
        }
    }
}