using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WampSharp.V2.Client;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// An object that reconnects to realm on connection loss.
    /// </summary>
    public class WampChannelReconnector : IDisposable
    {
        private IObservable<Unit> mMerged;
        private IDisposable mDisposable = Disposable.Empty;
        private bool mStarted = false;
        private readonly object mLock = new object();
        private readonly IDisposable mConnectionBrokenDisposable;

        /// <summary>
        /// Initializes a new instance of <see cref="WampChannelReconnector"/>.
        /// </summary>
        /// <param name="channel">The channel used to connect.</param>
        /// <param name="connector">The Task to use in order to connect.</param>
        public WampChannelReconnector(IWampChannel channel, Func<Task> connector)
        {
            IWampClientConnectionMonitor monitor = channel.RealmProxy.Monitor;

            var connectionBrokenObservable =
                Observable.FromEventPattern<WampSessionCloseEventArgs>
                          (x => monitor.ConnectionBroken += x,
                           x => monitor.ConnectionBroken -= x)
                          .Select(x => Unit.Default)
                          .Replay(1);

            var onceAndConnectionBroken =
                connectionBrokenObservable.StartWith(Unit.Default);

            IObservable<IObservable<Unit>> reconnect =
                from connectionBroke in onceAndConnectionBroken
                let tryReconnect = Observable.FromAsync(connector)
                    .Catch<Unit, Exception>(x => Observable.Empty<Unit>())
                select tryReconnect;

            mConnectionBrokenDisposable = connectionBrokenObservable.Connect();

            mMerged = reconnect.Concat();
        }

        /// <summary>
        /// Start trying connection establishment to router.
        /// </summary>
        public void Start()
        {
            lock (mLock)
            {
                if (mStarted)
                {
                    throw new Exception("Already started");
                }
                else
                {
                    if (mMerged != null)
                    {
                        mDisposable = mMerged.Subscribe(x => { });
                        mStarted = true;
                    }
                    else
                    {
                        throw new ObjectDisposedException(typeof (WampChannelReconnector).Name);
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (mLock)
            {
                mMerged = null;
                mDisposable.Dispose();
                mConnectionBrokenDisposable.Dispose();
            }
        }
    }
}