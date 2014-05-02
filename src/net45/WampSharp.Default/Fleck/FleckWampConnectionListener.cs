using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Fleck
{
    public class FleckWampConnectionListener<TMessage> : IWampConnectionListener<TMessage>
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject =
            new Subject<IWampConnection<TMessage>>();

        private IWebSocketServer mServer;
        private readonly string mLocation;
        private readonly IWampTextBinding<TMessage> mBinding;
        private readonly object mLock = new object();

        public FleckWampConnectionListener(string location,
                                           IWampTextBinding<TMessage> binding)
        {
            mLocation = location;
            mBinding = binding;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            lock (mLock)
            {
                if (mServer == null)
                {
                    StartServer();
                }

                return new CompositeDisposable(mSubject.Subscribe(observer),
                                               Disposable.Create(StopServerIfNeeded));
            }
        }

        private void StopServerIfNeeded()
        {
            lock (mLock)
            {
                if (!mSubject.HasObservers)
                {
                    mServer.Dispose();
                    mServer = null;
                }
            }
        }

        private void StartServer()
        {
            WebSocketServer server = new WebSocketServer(mLocation);
            mServer = server;
            server.SupportedSubProtocols = new[] {mBinding.Name};

            mServer.Start(connection =>
                              {
                                  FleckWampTextConnection<TMessage> wampConnection =
                                      new FleckWampTextConnection<TMessage>(connection,
                                                                            mBinding);

                                  OnNewConnection(wampConnection);
                              });
        }

        private void OnNewConnection(FleckWampConnection<TMessage> wampConnection)
        {
            mSubject.OnNext(wampConnection);
        }
    }
}